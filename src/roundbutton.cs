using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsFormsApplication1
{
	[Description("�p���ۂ��{�^��")]
	[DefaultEvent("Click")]
	public partial class RoundButton : UserControl
	{
		/// <summary>
		/// �e�̑傫��
		/// </summary>
		private int shadowSize = 6;

		/// <summary>
		/// �R�[�i�[�̊p�ۂ̃T�C�Y�i���a�j
		/// </summary>
		private int cornerR = 10;

		/// <summary>
		/// �{�^���\�ʂ̐F
		/// </summary>
		private Color surfaceColor = Color.SlateGray;

		/// <summary>
		/// �{�^���\�ʂ̃n�C���C�g�̐F
		/// </summary>
		private Color highLightColor = Color.White;

		/// <summary>
		/// �{�^����Ƀ}�E�X���������̘g�̐F
		/// </summary>
		private Color borderColor = Color.Orange;

		/// <summary>
		/// �{�^����Ƀt�H�[�J�X���������Ă��鎞�̘g�̐F
		/// </summary>
		private Color focusColor = Color.Blue;

		/// <summary>
		/// �{�^���̕�����
		/// </summary>
		private string buttonText = "RoundButton";

		/// <summary>
		/// �}�E�X��������Ă���Ԃ��� True
		/// </summary>
		private bool mouseDowning = false;

		//---------------------------------------------------------------------
		#region "�f�U�C�����O�����J�v���p�e�B"
		[Category("Appearance")]
        [Browsable(true)]
		[Description("�p�̊ۂ����w�肵�܂��B�i���a�j")]
		public int CornerR
		{
			get
			{
				return (int)(cornerR / 2);
			}
			set
			{
				if (value > 0)
					cornerR = value * 2;
				else
					throw new ArgumentException("Corner R", "0 �ȏ�̒l�����Ă��������B");

				RenewPadding();
				Refresh();
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		[Description("�{�^���\�ʂ̃n�C���C�g�̐F���w�肵�܂��B")]
		public Color HighLightColor
		{
			get
			{
				return highLightColor;
			}
			set
			{
				highLightColor = value;
				Refresh();
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		[Description("�{�^���\�ʂ̐F���w�肵�܂��B")]
		public Color SurfaceColor
		{
			get
			{
				return surfaceColor;
			}
			set
			{
				surfaceColor = value;
				Refresh();
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		[Description("�{�^����Ƀ}�E�X���������̘g�̐F���w�肵�܂��B")]
		public Color BorderColor
		{
			get
			{
				return borderColor;
			}
			set
			{
				borderColor = value;
				//Refresh();
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		[Description("�t�H�[�J�X�������������̘g�̐F���w�肵�܂��B")]
		public Color FocusColor
		{
			get
			{
				return focusColor;
			}
			set
			{
				focusColor = value;
				//Refresh();
			}
		}


		[Category("Appearance")]
		[Browsable(true)]
		[Description("�{�^���̕�������w�肵�܂��B")]
		public string ButtonText
		{
			get
			{
				return buttonText;
			}
			set
			{
				buttonText = value;
				Refresh();
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		[Description("�e�̑傫�����w�肵�܂��B")]
		public int ShadowSize
		{
			get
			{
				return shadowSize;
			}
			set
			{
				if (value >= 0)
					shadowSize = value;
				else
					throw new ArgumentException("ShadowSize", "0 �ȏ�̒l�����Ă��������B");

				RenewPadding();
				Refresh();
			}
		}

		#endregion
		//---------------------------------------------------------------------


		/// <summary>
		/// �R���X�g���N�^
		/// </summary>
		public RoundButton()
		{
            InitializeComponent();

			// �R���g���[���̃T�C�Y���ύX���ꂽ���� Paint �C�x���g�𔭐�������
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			//SetStyle(ControlStyles., true);

			this.BackColor = Color.Transparent;

			RenewPadding();
			//labelCaption.Text = "RoundButton";
		}

		/// <summary>
		/// �}�E�X Enter �C�x���g�̃I�[�o�[���C�h
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);

			Refresh();
			Graphics g = this.CreateGraphics();
			DrawButtonCorner(g, borderColor);
			g.Dispose();
		}

		/// <summary>
		/// �}�E�X Leave �C�x���g�̃I�[�o�[���C�h
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			Refresh();

			// �}�E�X������Ă��t�H�[�J�X�͎c���Ă��邱�Ƃ�����̂ŁA
			// ���̂Ƃ��̓t�H�[�J�X�p�̐F�Řg��h��
			if (this.Focused)
			{
				Graphics g = this.CreateGraphics();
				DrawButtonCorner(g, focusColor);
				g.Dispose();
			}
		}

		/// <summary>
		/// �}�E�X Down �C�x���g�̃I�[�o�[���C�h
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (!mouseDowning)
			{
				mouseDowning = true;
				Refresh();
				Graphics g = this.CreateGraphics();
				DrawButtonSurfaceDown(g);
				DrawButtonCorner(g, borderColor);
				g.Dispose();
			}

			base.OnMouseDown(e);
		}

		/// <summary>
		/// �}�E�X Up �C�x���g�̃I�[�o�[���C�h
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (mouseDowning)
			{
				mouseDowning = false;
				Refresh();
				Graphics g = this.CreateGraphics();
				DrawButtonCorner(g, borderColor);
				g.Dispose();
			}

			base.OnMouseUp(e);
		}

		/// <summary>
		/// �t�H�[�J�X������������
		/// </summary>
		/// <param name="e"></param>
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);

			if (!mouseDowning)
			{
				Graphics g = this.CreateGraphics();
				DrawButtonCorner(g, focusColor);
				g.Dispose();
			}
		}

		/// <summary>
		/// �t�H�[�J�X����������
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);

			Refresh();
		}

		/// <summary>
		/// �L�[�������ꂽ��
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			// �X�y�[�X�L�[�������ꂽ���́A�}�E�X Down �Ɠ����悤�ɏ�������
			if (!mouseDowning && e.KeyCode == Keys.Space)
			{
				mouseDowning = true;
				Refresh();
				Graphics g = this.CreateGraphics();
				DrawButtonSurfaceDown(g);
				g.Dispose();
			}

			base.OnKeyDown(e);
		}

		/// <summary>
		/// �L�[�������ꂽ��
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyUp(KeyEventArgs e)
		{
			// �X�y�[�X�L�[�������ꂽ���́A�}�E�X Up �Ɠ����悤�ɏ�������
			// �g�̐F�����̓t�H�[�J�X�̐F�ɂ���
			if (mouseDowning && e.KeyCode == Keys.Space)
			{
				mouseDowning = false;
				Refresh();
				Graphics g = this.CreateGraphics();
				DrawButtonCorner(g, focusColor);
				g.Dispose();
			}

			base.OnKeyUp(e);
		}

		/// <summary>
		/// Paint �C�x���g�̃I�[�o�[���C�h
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			//base.OnPaint(e);
			DrawButtonSurface(e.Graphics);
		}

		//---------------------------------------------------------------------
		#region "�v���C�x�[�g���\�b�h"

		/// <summary>
		/// Padding �T�C�Y�X�V
		/// </summary>
		private void RenewPadding()
		{
			int harfCornerR = (int)(cornerR / 2);
			int adjust = (int)(Math.Cos(45 * Math.PI / 180) * harfCornerR);
			this.Padding = new Padding(harfCornerR + shadowSize - adjust);
		}

		/// <summary>
		/// �{�^���̕������`��
		/// </summary>
		/// <param name="g"></param>
		/// <returns></returns>
		private void DrawText(Graphics g)
		{
			// �`��̈�̐ݒ�
			Rectangle rectangle = new Rectangle(this.Padding.Left,
												this.Padding.Top,
												this.Width - this.Padding.Left - this.Padding.Right,
												this.Height - this.Padding.Top - this.Padding.Bottom);
			
			// �����񂪕`��̈�Ɏ��܂�悤�ɒ���
			StringBuilder sb = new StringBuilder();
			StringBuilder sbm = new StringBuilder();

			foreach (char c in buttonText)
			{
				sbm.Append(c);
				Size size = TextRenderer.MeasureText(sbm.ToString(), this.Font);

				if (size.Width > rectangle.Width - this.Font.Size)
				{
					sbm.Remove(sbm.Length - 1, 1);
					sbm.Append(c);
					sbm.AppendLine("");
					sb.Append(sbm.ToString());
					sbm = new StringBuilder();
				}
			}
			sb.Append(sbm.ToString());

			// �����ς݂̕������`��
			TextRenderer.DrawText(g,
				sb.ToString(), 
				this.Font,
				rectangle, 
				this.ForeColor, 
				TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

		}

		/// <summary>
		/// �{�^���̕`��i���ݒ�
		/// </summary>
		private void SetSmoothMode(Graphics g)
		{
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			//g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
			g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
		}

		/// <summary>
		/// �e�p�̃p�X���擾
		/// </summary>
		/// <returns></returns>
		private GraphicsPath GetShadowPath()
		{
			GraphicsPath gp = new GraphicsPath();

			int w = this.Width - cornerR;
			int h = this.Height - cornerR;
			/*
			int ox = this.Width;
			int oy = this.Height;

			if (ox < oy)
			{
				float ratio = (float)shadowSize / oy;
				ox = shadowSize - (int)(ratio * ox);
				oy = 0;
			}
			else
			{
				float ratio = (float)shadowSize / ox;
				ox = 0;
				oy = shadowSize - (int)(ratio * oy);
			}

			gp.AddArc(ox, oy, cornerR, cornerR, 180, 90);
			gp.AddArc(w - ox, oy, cornerR, cornerR, 270, 90);
			gp.AddArc(w - ox, h - oy, cornerR, cornerR, 0, 90);
			gp.AddArc(ox, h - oy, cornerR, cornerR, 90, 90);
			gp.CloseFigure();
			*/
			gp.AddArc(0, 0, cornerR, cornerR, 180, 90);
			gp.AddArc(w, 0, cornerR, cornerR, 270, 90);
			gp.AddArc(w, h, cornerR, cornerR, 0, 90);
			gp.AddArc(0, h, cornerR, cornerR, 90, 90);
			gp.CloseFigure();

			return gp;
		}

		/// <summary>
		/// �e�p�̃u���V�擾
		/// </summary>
		/// <param name="graphicsPath"></param>
		/// <returns></returns>
		private PathGradientBrush GetShadowBrush(GraphicsPath graphicsPath)
		{
			PathGradientBrush brush = new PathGradientBrush(graphicsPath);
			ColorBlend colorBlend = new ColorBlend();
			float pos = 0;

			if (this.Width < this.Height)
				pos = ((float)shadowSize * 2 / this.Height);
			else
				pos = ((float)shadowSize * 2 / this.Width);

			colorBlend.Positions = new float[3] { 0.0f, pos, 1.0f };

			colorBlend.Colors = new Color[3] { 
					Color.FromArgb(0, Color.White), 
					Color.FromArgb(20, 0, 0, 0),
					Color.FromArgb(20, 0, 0, 0)
			};

			brush.CenterColor = Color.Black;
			brush.CenterPoint = new PointF(this.Width / 2, this.Height / 2);
			brush.InterpolationColors = colorBlend;

			return brush;
		}

		/// <summary>
		/// �{�^���̕\�ʕ`��
		/// </summary>
		/// <param name="g"></param>
		private void DrawButtonSurface(Graphics g)
		{
			// �`��i���ݒ�
			SetSmoothMode(g);
			
			// �ϐ�������
			int offset = shadowSize;
			int w = this.Width - cornerR;
			int h = this.Height - cornerR;
			int harfHeight = (int)(this.Height / 2);

			// �e�p�̃p�X������
			GraphicsPath shadowPath = null;
			if (shadowSize > 0)
				shadowPath = GetShadowPath();

			// �{�^���̕\�ʂ̃p�X������
			GraphicsPath graphPath = new GraphicsPath();
			graphPath.AddArc(offset, offset, cornerR, cornerR, 180, 90);
			graphPath.AddArc(w - offset, offset, cornerR, cornerR, 270, 90);
			graphPath.AddArc(w - offset, h - offset, cornerR, cornerR, 0, 90);
			graphPath.AddArc(offset, h - offset, cornerR, cornerR, 90, 90);
			graphPath.CloseFigure();

			// �{�^���̃n�C���C�g�����̃p�X������
			offset += 1;
			//cornerR -= 1;
			GraphicsPath graphPath2 = new GraphicsPath();
			graphPath2.AddArc(offset, offset, cornerR, cornerR, 180, 90);
			graphPath2.AddArc(w - offset, offset, cornerR, cornerR, 270, 90);
			graphPath2.AddLine(this.Width - offset, offset + (int)(cornerR / 2), this.Width - offset, harfHeight);
			graphPath2.AddLine(offset, harfHeight, offset, harfHeight);
			graphPath2.CloseFigure();


			// �e�p�̃u���V������
			PathGradientBrush shadowBrush = null;
			if (shadowSize > 0)
				shadowBrush = GetShadowBrush(shadowPath);

			// �{�^���̕\�ʗp�̃u���V������
			Brush fillBrush2 = new SolidBrush(surfaceColor);

			// �{�^���̃n�C���C�g�����p�̃u���V������
			LinearGradientBrush fillBrush = new LinearGradientBrush(new Point(0, 0),
													new Point(0, harfHeight + 1),
													Color.FromArgb(255, highLightColor),
													Color.FromArgb(0, surfaceColor));

			// �e �� �\�� �� �n�C���C�g�̏��ԂŃp�X��h��
			if (shadowSize > 0)
				g.FillPath(shadowBrush, shadowPath);
			g.FillPath(fillBrush2, graphPath);
			g.FillPath(fillBrush, graphPath2);

			// �{�^���̕�����`��
			DrawText(g);

			// �㏈��
			if (shadowSize > 0)
				shadowPath.Dispose();
			graphPath.Dispose();
			graphPath2.Dispose();

			if (shadowSize > 0)
				shadowBrush.Dispose();
			fillBrush.Dispose();
			fillBrush2.Dispose();
		}

		/// <summary>
		/// �{�^���̕\�ʕ`��@�i�}�E�X Down �C�x���g���j
		/// </summary>
		/// <param name="g"></param>
		private void DrawButtonSurfaceDown(Graphics g)
		{
			// �`��i���ݒ�
			SetSmoothMode(g);

			// �ϐ�������
			int offset = shadowSize;
			int w = this.Width - cornerR;
			int h = this.Height - cornerR;


			// �{�^���̕\�ʗp�̃u���V������
			Brush fillBrush = new SolidBrush(Color.FromArgb(30, Color.Black));

			GraphicsPath graphPath = new GraphicsPath();
			graphPath.AddArc(offset, offset, cornerR, cornerR, 180, 90);
			graphPath.AddArc(w - offset, offset, cornerR, cornerR, 270, 90);
			graphPath.AddArc(w - offset, h - offset, cornerR, cornerR, 0, 90);
			graphPath.AddArc(offset, h - offset, cornerR, cornerR, 90, 90);
			graphPath.CloseFigure();

			g.FillPath(fillBrush, graphPath);

			graphPath.Dispose();
			fillBrush.Dispose();
		}

		/// <summary>
		/// �}�E�X���{�^���̗̈�ɓ��������ɁA�g�ɐF�����鏈��
		/// �ƁA�t�H�[�J�X���������Ă��鎞�ɁA�g�ɐF�����鏈���p
		/// </summary>
		/// <param name="g"></param>
		private void DrawButtonCorner(Graphics g, Color color)
		{
			// �`��i���ݒ�
			SetSmoothMode(g);

			// �ϐ�������
			int offset = shadowSize;
			int w = this.Width - cornerR;
			int h = this.Height - cornerR;

			// �y���̏�����
			Pen cornerPen = new Pen(Color.FromArgb(128, color), 2);
			//Pen cornerPen = new Pen(color, 1.5f);

			// �`��̈�̏�����
			GraphicsPath graphPath = new GraphicsPath();
			graphPath.AddArc(offset, offset, cornerR, cornerR, 180, 90);
			graphPath.AddArc(w - offset, offset, cornerR, cornerR, 270, 90);
			graphPath.AddArc(w - offset, h - offset, cornerR, cornerR, 0, 90);
			graphPath.AddArc(offset, h - offset, cornerR, cornerR, 90, 90);
			graphPath.CloseFigure();

			// �`��
			g.DrawPath(cornerPen, graphPath);

			// �㏈��
			graphPath.Dispose();
			cornerPen.Dispose();
		}

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // RoundButton
            // 
            this.Name = "RoundButton";
            this.Load += new System.EventHandler(this.RoundButton_Load);
            this.ResumeLayout(false);

        }

        private void RoundButton_Load(object sender, EventArgs e)
        {

        }

        #endregion

        //---------------------------------------------------------------------

    }
}