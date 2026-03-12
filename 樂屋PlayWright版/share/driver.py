from selenium import webdriver
from webdriver_manager.chrome import ChromeDriverManager
import time

def set_driver():
    options = webdriver.ChromeOptions()
    options.add_experimental_option("detach", True) #不自動關閉driver
    # options.add_argument('blink-settings=imagesEnabled=false')  #設定不加載圖片
    # options.add_argument("--headless") #設定不顯示chrome
    prefs = {
        'profile.default_content_setting_values' :  {    
            'notifications' : 2  
        }  
    }  
    options.add_experimental_option('prefs',prefs)              #關閉彈窗對話框
    driver_path = ChromeDriverManager().install()
    driver = webdriver.Chrome(driver_path,options=options)
    driver.set_window_size(1280, 1024)
    return driver