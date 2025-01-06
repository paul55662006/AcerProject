# -*- coding: utf-8 -*-
"""
Created on Wed Jan  1 22:33:36 2025

@author: User
"""
import pyodbc
import pandas as pd
import numpy as np
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
    query = "SELECT TimeSlot, Date FROM Clients;"
    df = pd.read_sql(query, conn)
    conn.close()
    return df

# 長條圖 (TimeSlot)
def plot_time_slot_distribution_by_month():
    df = fetch_data_to_dataframe()
    df['Date'] = pd.to_datetime(df['Date'])  # 將 Date 轉為 datetime
    df['YearMonth'] = df['Date'].dt.to_period('M')  # 年月作為 X 軸
    start_month = pd.to_datetime('2024-12')
    end_month = start_month + pd.DateOffset(months=6)
    full_range = pd.period_range(start=start_month, end=end_month, freq='M')

    grouped = df.groupby(['YearMonth', 'TimeSlot']).size().unstack(fill_value=0)
    grouped = grouped.reindex(index=full_range, fill_value=0)
    bar_width = 0.35
    index = np.arange(len(grouped.index))

    plt.figure(figsize=(12, 8))
    colors = ['orange', 'mediumaquamarine', 'gray']  # 定義每個 TimeSlot 的顏色
    for i, time_slot in enumerate(grouped.columns):
        plt.bar(
            index + i * bar_width, 
            grouped[time_slot], 
            bar_width, 
            label=f"時段 {time_slot}",  # 使用原始 TimeSlot 值作為標籤
            color=colors[i % len(colors)],
        )      
        # 標籤
        for x, y in zip(index + i * bar_width, grouped[time_slot]):
            if y > 0:
                plt.text(x, y + 0.5, str(y), ha='center', va='bottom', fontsize=10, color='dimgray')

    plt.grid(axis='y', linestyle='--', linewidth=0.7, alpha=0.7)
    plt.title('每月看診時段數量統計', {'fontsize': 20, 'color': 'dimgray', 'fontweight': 'bold'}, pad=5)
    plt.xlabel('年/月', fontsize=12, color='dimgray')
    plt.ylabel('總計', fontsize=12, color='dimgray')
    plt.xticks(ticks=index + bar_width, labels=[str(x) for x in grouped.index], rotation=45, color='dimgray')
    plt.legend(title='時段', fontsize=10, loc='upper left')

    # 外框
    ax = plt.gca()
    ax.tick_params(axis='y', colors='dimgray')
    ax.yaxis.label.set_color('dimgray')
    ax.spines['top'].set_color('dimgray')     
    ax.spines['right'].set_color('dimgray') 
    ax.spines['bottom'].set_color('dimgray') 
    ax.spines['left'].set_color('dimgray')  

    # 調整外框線條寬度
    for spine in ax.spines.values():
        spine.set_linewidth(1)
    plt.tight_layout()

    # 保存圖片
    plt.savefig("img/time_slot.png", dpi=300, bbox_inches='tight')

    plt.close()

# 主函數
if __name__ == "__main__":
    plot_time_slot_distribution_by_month()

