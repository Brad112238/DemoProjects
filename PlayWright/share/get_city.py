import json

def get_city_code():
    with open("./test.txt","r" , encoding="utf-8")as f:
        data = f.read()
        city = json.loads(data)

    return city["CITY_CODE"]

english_to_chinese={
    "Taipei-city" : "台北市",
}