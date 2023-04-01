#平滑化でspline
use_spline = TRUE

#入力各変数の特徴量（平均、分散、etc)を特徴量にする場合のlookback数
lookback=200

#特徴量平滑化
feature_smooth_window = 0

#一度に送られてくるデータ長
one_input = 200

#入力データ平滑化
smooth_window = 50
smooth_window_slide = 1

#予測に用いる最大データ長
max_data_len = max(one_input*10, smooth_window*3)

#予測モデル訓練に使う直前の点数
train_num = 200
#monotonicity計算に使う直前の点数
monotonicity_num = 200


#デフォルトの閾値
threshold = 0

#plot用Ymax
ymax = threshold*1.5

#各特徴量毎の閾値、Ymaxのパラメータセット
feature_param = NULL

#予測する未来の長さ閾値
max_prediction_length = 20000

#送られてくるデータの最大保持長さ
max_retained_length = 160000

#入力データの分解能
unit_of_time = "sec"
unit_of_record = 70

#予測された現在から測定した異常発生時間
failure_time_init = max_prediction_length*unit_of_record
failure_time = failure_time_init

#出力時の時間単位
forecast_time_unit = "hour"

#異常度モデル
m_mahalanobis <- NULL

#送られてくるデータの全てで過去から現在まで
#予測に用いる最大データ長までに制限したデータフレーム
pre = NULL
pre_org = NULL
#最大保持長までに制限したデータフレーム
past = NULL

#予測モデル選択
use_auto_arima = F
use_arima = T
use_ets = F
use_plophet = F


#異常を含んだデータがinputできた場合=TRUE
abnormality_detected_data <- TRUE

#追跡していく特徴量
tracking_feature <- NULL
dynamic_threshold = TRUE
watch_name = "X793.."

RUL <- c()
pre = NULL
past = NULL
feature_param = NULL

index_number <- 0
time_Index <- 1

timeStamp <- ""
save.image("./predictive_maintenance.RData")
