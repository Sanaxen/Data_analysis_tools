namespace EncryptString
{
    public partial class EncryptStringCs
    {
        public EncryptStringCs()
        {

        }

        /// <summary>
        /// ��������Í�������
        /// </summary>
        /// <param name="sourceString">�Í������镶����</param>
        /// <param name="password">�Í����Ɏg�p����p�X���[�h</param>
        /// <returns>�Í������ꂽ������</returns>
        public string EncryptString(string sourceString, string password)
        {
            //RijndaelManaged�I�u�W�F�N�g���쐬
            System.Security.Cryptography.RijndaelManaged rijndael =
                new System.Security.Cryptography.RijndaelManaged();

            //�p�X���[�h���狤�L�L�[�Ə������x�N�^���쐬
            byte[] key, iv;
            GenerateKeyFromPassword(
                password, rijndael.KeySize, out key, rijndael.BlockSize, out iv);
            rijndael.Key = key;
            rijndael.IV = iv;

            //��������o�C�g�^�z��ɕϊ�����
            byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(sourceString);

            //�Ώ̈Í����I�u�W�F�N�g�̍쐬
            System.Security.Cryptography.ICryptoTransform encryptor =
                rijndael.CreateEncryptor();
            //�o�C�g�^�z����Í�������
            byte[] encBytes = encryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
            //����
            encryptor.Dispose();

            //�o�C�g�^�z��𕶎���ɕϊ����ĕԂ�
            return System.Convert.ToBase64String(encBytes);
        }

        /// <summary>
        /// �Í������ꂽ������𕜍�������
        /// </summary>
        /// <param name="sourceString">�Í������ꂽ������</param>
        /// <param name="password">�Í����Ɏg�p�����p�X���[�h</param>
        /// <returns>���������ꂽ������</returns>
        public string DecryptString(string sourceString, string password)
        {
            //RijndaelManaged�I�u�W�F�N�g���쐬
            System.Security.Cryptography.RijndaelManaged rijndael =
                new System.Security.Cryptography.RijndaelManaged();

            //�p�X���[�h���狤�L�L�[�Ə������x�N�^���쐬
            byte[] key, iv;
            GenerateKeyFromPassword(
                password, rijndael.KeySize, out key, rijndael.BlockSize, out iv);
            rijndael.Key = key;
            rijndael.IV = iv;

            //��������o�C�g�^�z��ɖ߂�
            byte[] strBytes = System.Convert.FromBase64String(sourceString);

            //�Ώ̈Í����I�u�W�F�N�g�̍쐬
            System.Security.Cryptography.ICryptoTransform decryptor =
                rijndael.CreateDecryptor();
            //�o�C�g�^�z��𕜍�������
            //�������Ɏ��s����Ɨ�OCryptographicException������
            byte[] decBytes = decryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
            //����
            decryptor.Dispose();

            //�o�C�g�^�z��𕶎���ɖ߂��ĕԂ�
            return System.Text.Encoding.UTF8.GetString(decBytes);
        }

        /// <summary>
        /// �p�X���[�h���狤�L�L�[�Ə������x�N�^�𐶐�����
        /// </summary>
        /// <param name="password">��ɂȂ�p�X���[�h</param>
        /// <param name="keySize">���L�L�[�̃T�C�Y�i�r�b�g�j</param>
        /// <param name="key">�쐬���ꂽ���L�L�[</param>
        /// <param name="blockSize">�������x�N�^�̃T�C�Y�i�r�b�g�j</param>
        /// <param name="iv">�쐬���ꂽ�������x�N�^</param>
        private  void GenerateKeyFromPassword(string password,
            int keySize, out byte[] key, int blockSize, out byte[] iv)
        {
            //�p�X���[�h���狤�L�L�[�Ə������x�N�^���쐬����
            //salt�����߂�
            byte[] salt = System.Text.Encoding.UTF8.GetBytes("salt�͕K��8�o�C�g�ȏ�");
            //Rfc2898DeriveBytes�I�u�W�F�N�g���쐬����
            System.Security.Cryptography.Rfc2898DeriveBytes deriveBytes =
                new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt);
            //.NET Framework 1.1�ȉ��̎��́APasswordDeriveBytes���g�p����
            //System.Security.Cryptography.PasswordDeriveBytes deriveBytes =
            //    new System.Security.Cryptography.PasswordDeriveBytes(password, salt);
            //���������񐔂��w�肷�� �f�t�H���g��1000��
            deriveBytes.IterationCount = 1000;

            //���L�L�[�Ə������x�N�^�𐶐�����
            key = deriveBytes.GetBytes(keySize / 8);
            iv = deriveBytes.GetBytes(blockSize / 8);
        }
    }
}