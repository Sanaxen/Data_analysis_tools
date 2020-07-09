crosstable <- function(data, xname, yname, format = "html", xlabel = xname, ylabel = yname, caption = NULL){
  library(stringr)
  library(tidyr)
  library(knitr)
  library(kableExtra)

  # 0. もし入力されたデータが数値型なら文字列型にしてから扱う
  if(is.numeric(data[[xname]])) data[[xname]] <- as.character(data[[xname]])
  if(is.numeric(data[[yname]])) data[[yname]] <- as.character(data[[yname]])

  # 1. クロス集計表をdataframeとして作る
  ## 度数のクロス集計表
  table_df_n <- data %>% group_by_(xname, yname) %>% count() %>% spread_(xname, "n")
  ## 比率（セル比率）のクロス集計表
  table_df_p <- data %>% group_by_(xname, yname) %>% count() %>%
    mutate(p = round(n/nrow(data),3)*100) %>%
    select(-n) %>% spread_(xname, "p")

  ## NAを0で埋める
  table_df_n[is.na(table_df_n)] <- 0
  table_df_p[is.na(table_df_p)] <- 0

  ## 比率のクロス集計表に"%"記号をつける
  for(i in 1:nrow(table_df_p)){
    table_df_p[i,-1] <- paste0(table_df_p[i,-1],'%')
  }

  ## 実数と比率の文字列ベクトルを連結させ，「〇〇(〇〇%)」の形にする
  table_df = table_df_n
  for(i in 1:ncol(table_df_n)) {
    if(i == 1) table_df[i] = table_df_n[i]
    if(i != 1) table_df[i] = str_c(table_df_n[[i]], " (",table_df_p[[i]], ")")
  }

  ## 「合計」列を追加する
  sum_col_n <- apply(table_df_n[-1], 1, sum)
  sum_col_p <- paste0(round(sum_col_n / sum(sum_col_n),3)*100,'%')
  table_df['合計'] = str_c(sum_col_n, " (",sum_col_p, ")")

  ## 「合計」行を追加する
  sum_row_n <- apply(table_df_n[-1], 2, sum)
  sum_row_p <- paste0(round(sum_row_n / sum(sum_row_n),3)*100,'%')
  sum_row <- str_c(sum_row_n, " (",sum_row_p, ")")
  sum_col_sum <- str_c(sum(sum_col_n), " (100%)")
  sum_row <- data.frame(' ', matrix(as.character(sum_row), nrow = 1), sum_col_sum, stringsAsFactors = F)
  names(sum_row) <- colnames(table_df)
  table_df <- bind_rows(table_df, sum_row) # 最下行に追加

  # 2. kableExtraでの出力に備えて情報を付与
  ## y軸ラベルの作成
  table_df['label'] <- ylabel
  table_df[nrow(table_df),'label'] <- '合計'
  table_df <- table_df %>% select(label, colnames(table_df[-ncol(table_df)])) %>% rename(` ` = yname, ` ` = "label")

  ## add_header_above()の引数である"header"のためのベクトルを作成
  header_vec <- c(" " = 2, ncol(table_df)-3, " ")
  names(header_vec)[2] <- xlabel

  ## kableとkableExtraで表を作る
  k <- kable(table_df, format = format, align = "c", caption = caption) %>%
    kable_styling(full_width = F) %>%
    column_spec(c(1,2), bold = T) %>%
    collapse_rows(columns = 1, valign = "middle") %>% # セルの縦方向結合
    add_header_above(header = header_vec) # セルの横方向結合
    
    save_kable(k, "crosstable.png")
    #save_kable(k, "crosstable.html")
    #as_image(k, width = 8, file = "tmp.html")
  return(k)
}