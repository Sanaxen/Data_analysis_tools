#coding:UTF-8
import requests,os

token="#######################################"

#------画像を送る場合----------------------------
def send_image(image):
    url = "https://notify-api.line.me/api/notify"
    #token = "*********************************"
    headers = {"Authorization" : "Bearer "+ token}

    message = 'Fortnite！'
    payload = {"message" :  message}
    files = {"imageFile":open(image,'rb')}
    post = requests.post(url ,headers = headers ,params=payload,files=files)


#------メッセージを送る場合----------------------------
def send_msg(msg):
    url = "https://notify-api.line.me/api/notify"
    #token = "*********************************"
    headers = {"Authorization" : "Bearer "+ token}

    message = 'Fortnite！'
    payload = {"message" :  msg}

    r = requests.post(url ,headers = headers ,params=payload)


if __name__ == '__main__':
    send_image('Digraph.png')
    