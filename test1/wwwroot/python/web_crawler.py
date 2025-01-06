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
import requests

#強制讓python輸出成utf-8, ASP.NET可以調用
sys.stdout.reconfigure(encoding='utf-8')

# 手動指定前12筆資料的圖片連結
manual_image_links = [
    "https://www.orton555.url.tw/images/product_1558411.jpg",
    "https://www.orton555.url.tw/images/product_1558409.jpg",
    "https://www.orton555.url.tw/images/product_1558407.jpg",
    "https://www.orton555.url.tw/images/product_1558406.jpg",
    "https://www.orton555.url.tw/images/product_1558404.jpg",
    "https://www.orton555.url.tw/images/product_1558401.jpg",
    "https://www.orton555.url.tw/images/product_1558398.jpg",
    "https://www.orton555.url.tw/images/product_1558397.jpg",
    "https://www.orton555.url.tw/images/product_1558393.jpg",
    "https://www.orton555.url.tw/images/product_1557261.jpg",
    "https://www.orton555.url.tw/images/product_1557260.jpg",
    "https://www.orton555.url.tw/images/product_1557255.jpg"
]

# 設定目標網址
url = "https://www.orton555.url.tw/product.html"

try:
    # 發送 GET 請求
    response = requests.get(url)
    response.encoding = 'utf-8'
    
    if response.status_code == 200:
        soup = BeautifulSoup(response.text, 'html.parser')
        products_data = []  # 儲存產品資料
        
        products = soup.find_all('div', class_='product')
        
        for idx, product in enumerate(products):
            try:
                # 產品名稱
                name = ''
                name_patterns = [r'.*處方罐頭.*', r'.*保健罐頭.*', r'.*處方肉泥.*', r'.*保健肉泥.*']
                for pattern in name_patterns:
                    name_elem = product.find(string=re.compile(pattern))
                    if name_elem:
                        name = name_elem.strip()
                        break
                
                # 價格
                price = ''
                price_patterns = [r'.*\$.*\$.*\$.*', r'建議售價:.*\(7入\)', r'建議售價:.*']
                for pattern in price_patterns:
                    price_elem = product.find(string=re.compile(pattern))
                    if price_elem:
                        price = price_elem.strip()
                        break
                
                # 產品描述
                description = []
                for label in ['主要原料:', '營養添加:', '功效:', '使用方法:']:
                    elem = product.find(string=re.compile(label))
                    if elem:
                        next_text = elem.find_next(string=True)
                        if next_text:
                            description.append(f"{label} {next_text.strip()}")
                
                # 產品規格
                weight_patterns = [r'淨重:\s*\d+g\/罐', r'每份\d+g\/每包含[一\d]份', r'每條\d+g']
                for pattern in weight_patterns:
                    weight_elem = product.find(string=re.compile(pattern))
                    if weight_elem:
                        description.append(f"規格: {weight_elem.strip()}")
                
                full_description = '\n'.join(description)
                
                # 圖片連結（前12筆使用手動補充，其他使用爬取）
                if idx < len(manual_image_links):
                    img_url = manual_image_links[idx]
                else:
                    img_url = 'N/A'
                    img_tag = product.find('img')
                    if img_tag:
                        img_src = img_tag.get('src') or img_tag.get('data-src')
                        if img_src:
                            if img_src.startswith('http'):
                                img_url = img_src
                            else:
                                img_url = f"https://www.orton555.url.tw/{img_src}" if img_src.startswith('images/') else f"https://www.orton555.url.tw/{img_src}"
                
                # 將資料存入列表
                products_data.append({
                    "產品名稱": name or "N/A",
                    "價格": price or "N/A",
                    "產品介紹": full_description or "N/A",
                    "圖片連結": img_url
                })
                # print(f"已爬取：{name}，圖片連結：{img_url}")
            
            except Exception as e:
                print(f"處理產品時發生錯誤：{e}")
        
        # 儲存為 JSON 檔
        # with open('Mao_Shanyiban_Series.json', 'w', encoding='utf-8') as json_file:
        #     json.dump(products_data, json_file, ensure_ascii=False, indent=4)
        
        # 轉換為 CSV 檔
        # df = pd.DataFrame(products_data)
        # df.to_csv('Mao_Shanyiban_Series.csv', index=False, encoding='utf-8-sig')
        
        # print(f"爬取完成！共取得 {len(products_data)} 筆資料")
        # print("資料已儲存為 JSON 和 CSV 格式。")
    
    else:
        print(f"HTTP 請求失敗，狀態碼：{response.status_code}")

except Exception as e:
    print(f"發生錯誤：{e}")

finally:
    print(json.dumps(products_data, ensure_ascii=False))
