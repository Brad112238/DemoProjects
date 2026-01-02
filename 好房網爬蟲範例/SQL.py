import pymssql
from datetime import datetime

server = ''
database = '' 
username = ''
password = ''

conn = pymssql.connect(server=server, user=username, password=password, database=database)

def insert_house_data(data):
    set_clause = ", ".join([f"{key}" for key in data.keys()]) 
    set_clause2 = ", ".join([f"%s" for key in data.keys()])
    sql = f"""
    INSERT INTO Tmp (
        {set_clause}
    ) VALUES (
        {set_clause2}
    )
    """

    def format_date(date_str):
        try:
            return datetime.strptime(date_str, '%Y%m%d_%H%M').strftime('%Y-%m-%d %H:%M:%S')
        except ValueError:
            return None
        
    data["CreateDateTime"] = format_date(data["CreateDateTime"])
    data["RefreshDateTime"] = format_date(data["RefreshDateTime"])
    data["LastRefreshDetail"] = format_date(data["LastRefreshDetail"])
    data["City"] = data["City"].replace("臺" , "台")
    data["Area"] = data["Area"].replace("臺" , "台")
    data["Street"] = data["Street"].replace("臺" , "台")

    data["Parking"] = 1 if data["Parking"] else 0
    data["Process"] = 1 if data["Process"] else 0
    values = [data[key] for key in data.keys()]

    with conn.cursor() as cursor:
        try:
            cursor.execute(sql, values)
            conn.commit()
            
        except Exception as e:
            print(f"發生錯誤: {e}")
            conn.rollback()

def get_OID_list(city):
    batchId = datetime.today().strftime("%Y%m%d")
    cursor = conn.cursor()

    query = "SELECT Oid FROM Tmp WHERE BatchId = %s AND City = %s AND CId = %s"
    cursor.execute(query, (batchId, city, 16))
    
    Oid_list = [row[0] for row in cursor.fetchall()]
    return Oid_list