# -*- coding: utf-8 -*-
"""
Created on Wed Jan  1 22:33:36 2025

@author: User
"""

import pyodbc
import pandas as pd
import matplotlib.pyplot as plt
from matplotlib import rcParams

# 資料庫連線設定
SERVER = r'frankdbserver.database.windows.net'
DATABASE = 'Reserve'
USERNAME = 'frankfan'
PASSWORD = 'Ab16881688?'

connection_string = f'DRIVER={{ODBC Driver 18 for SQL Server}};SERVER={SERVER};DATABASE={DATABASE};UID={USERNAME};PWD={PASSWORD}'

# Matplotlib 中文字型設定
rcParams['font.sans-serif'] = ['Microsoft JhengHei']  # 微軟正黑體
rcParams['axes.unicode_minus'] = False 

# 讀取資料(Pandas DataFrame)
def fetch_data_to_dataframe():
    conn = pyodbc.connect(connection_string)
    query = "SELECT PetType, COUNT(*) AS Count FROM Clients GROUP BY PetType;"
    df = pd.read_sql(query, conn)
    conn.close()
    return df

#圓餅圖 (PetType)
def plot_pet_type_distribution():

    df = fetch_data_to_dataframe()
    labels = df['PetType'].tolist()
    data = df['Count'].tolist()
    total = sum(data)

    # 自定義顏色
    colors = ['mediumaquamarine', 'yellow', 'yellowgreen', 'lightgreen', 'mediumseagreen']

    plt.figure(figsize=(8, 8))
    autopct_func = lambda p: f'{p:.1f}%\n({int(p * total / 100)})'  # 顯示百分比與數量
    plt.pie(data, 
            labels=labels, 
            autopct=autopct_func, 
            startangle=140, 
            colors=colors,
            wedgeprops={'edgecolor': 'white', 'linewidth': 2}, 
            textprops={'fontweight': 'bold', 'fontsize': 14, 'color': 'darkslategray'})
    plt.title('本月寵物類型統計', fontsize=20, fontweight='bold', color='dimgray', pad=20)

    # 保存
    output_file = "pet_types.png"
    plt.savefig("img/pet_types.png", dpi=300, bbox_inches='tight')

if __name__ == "__main__":
    plot_pet_type_distribution()
