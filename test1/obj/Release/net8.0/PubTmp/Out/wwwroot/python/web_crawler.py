

#導入必要的套件
from selenium import webdriver #導入Selenium網頁自動化工具
from selenium.webdriver.chrome.service import Service #導入Chrome瀏覽器服務
from selenium.webdriver.common.by import By #導入定位元素的方法
from selenium.webdriver.support.ui import WebDriverWait #導入等待頁面加載的工具
from selenium.webdriver.support import expected_conditions as EC #導入判斷頁面狀態的條件
from webdriver_manager.chrome import ChromeDriverManager #導入Chrome驅動程式管理器
from bs4 import BeautifulSoup #導入網頁解析工具
import pandas as pd #導入資料處理工具
import time #導入時間處理工具
import re #導入正則表達式工具
import sys
import json


#強制讓python輸出成utf-8, ASP.NET可以調用
sys.stdout.reconfigure(encoding='utf-8')

#設定Chrome選項
options = webdriver.ChromeOptions()
options.add_argument('--headless') #設定無介面模式
options.add_argument('--disable-gpu') #停用GPU加速
options.add_argument('--no-sandbox') #停用沙箱模式
options.add_argument('--disable-dev-shm-usage') #停用共享內存使用

#初始化瀏覽器
driver = webdriver.Chrome(service=Service(ChromeDriverManager().install()), options=options)
driver.set_page_load_timeout(30) #設定頁面載入超時時間為30秒
url = "https://www.orton555.url.tw/product.html" #設定網址
try:
    driver.get(url) #訪問網頁
    time.sleep(5) #等待5秒確保頁面載入完成
    soup = BeautifulSoup(driver.page_source, 'html.parser') #使用BeautifulSoup解析網頁內容
    products_data = [] #建立儲存產品資料的列表
    products = soup.find_all('div', class_='product') #找到所有產品容器
    
    #遍歷每個產品
    for product in products:
        try:
            name = '' #使用正則表達式尋找符合特定模式的產品名稱
            name_patterns = [
                r'.*處方罐頭.*', #尋找包含「處方罐頭」的文字
                r'.*保健罐頭.*', #尋找包含「保健罐頭」的文字
                r'.*處方肉泥.*', #尋找包含「處方肉泥」的文字
                r'.*保健肉泥.*'] #尋找包含「保健肉泥」的文字
            
            #尋找匹配的產品名稱
            for pattern in name_patterns:
                name_elem = product.find(string=re.compile(pattern))
                if name_elem:
                    name = name_elem.strip()
                    break           
            #尋找價格資訊（包含各種包裝規格）
            price = ''
            price_patterns = [
                r'.*\$.*\$.*\$.*',  #匹配多個價格的模式
                r'建議售價:.*\(7入\)',  #匹配7入裝的價格
                r'建議售價:.*']  #匹配一般價格
            for pattern in price_patterns:
                price_elem = product.find(string=re.compile(pattern))
                if price_elem:
                    price = price_elem.strip()
                    break            
            description = [] #擷取產品描述相關資訊與產品介紹
            
            #尋找並添加主要原料資訊
            ingredients = product.find(string=re.compile(r'主要原料:'))
            if ingredients:
                next_text = ingredients.find_next(string=True)
                if next_text:
                    description.append(f"主要原料: {next_text.strip()}")            
            
            #尋找並添加營養添加資訊
            nutrition = product.find(string=re.compile(r'營養添加:'))
            if nutrition:
                next_text = nutrition.find_next(string=True)
                if next_text:
                    description.append(f"營養添加: {next_text.strip()}")            
            
            #尋找並添加功效資訊
            effects = product.find(string=re.compile(r'功效:'))
            if effects:
                next_text = effects.find_next(string=True)
                if next_text:
                    description.append(f"功效: {next_text.strip()}")            
            
            #尋找並添加使用方法
            usage = product.find(string=re.compile(r'使用方法:'))
            if usage:
                next_text = usage.find_next(string=True)
                if next_text:
                    description.append(f"使用方法: {next_text.strip()}")           
            
            #尋找產品規格資訊
            weight_patterns = [
                r'淨重:\s*\d+g\/罐', #匹配罐裝重量
                r'每份\d+g\/每包含[一\d]份', #匹配包裝份量
                r'每條\d+g'] #匹配條狀包裝重量
            for pattern in weight_patterns:
                weight_elem = product.find(string=re.compile(pattern))
                if weight_elem:
                    description.append(f"規格: {weight_elem.strip()}")            
            full_description = '\n'.join(description) #合併所有產品介紹內容
            img = product.find('img') #擷取產品圖片連結
            if img:
                img_src = img.get('src', '')
                if img_src.startswith('images/'):
                    img_url = f"https://www.orton555.url.tw/{img_src}"
                else:
                    img_url = img_src
            else:
                img_url = 'N/A'           
            
            #將產品資訊加入列表
            products_data.append({
                "產品名稱": name or 'N/A',
                "價格": price or 'N/A',
                "產品介紹": full_description or 'N/A',
                "圖片連結": img_url})
            #print(f"已爬取：{name}")            
        except Exception as e:
            print(f"處理產品時發生錯誤：{e}")    
    
    #將資料儲存為CSV檔案
    #if products_data:
        #output_path = 'Mao_Shanyiban_Series.csv'
        #df = pd.DataFrame(products_data)
        #df.to_csv(output_path, index=False, encoding='utf-8-sig')
        #print(f"爬取完成！共取得 {len(products_data)} 筆資料")
        #print(f"資料已儲存至：{output_path}")
    #else:
        #print("未找到任何產品資料")
except Exception as e:
    print(f"執行過程發生錯誤：{e}")
finally:
    data = products_data[:7]
    print(json.dumps(data, ensure_ascii=False))
    driver.quit() #關閉瀏覽器