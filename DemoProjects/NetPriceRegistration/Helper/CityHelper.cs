using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseGo.Tools.PriceRegistration.Helper
{
    public static class CityHelper
    {
        private static readonly Dictionary<char, string> CountyCode = new()
        {
            ['A'] = "台北市",
            ['B'] = "台中市",
            ['C'] = "基隆市",
            ['D'] = "台南市",
            ['E'] = "高雄市",
            ['F'] = "新北市",
            ['G'] = "宜蘭縣",
            ['H'] = "桃園市",
            ['I'] = "嘉義市",
            ['J'] = "新竹縣",
            ['K'] = "苗栗縣",
            ['L'] = "台北縣(已廢止)",
            ['M'] = "南投縣",
            ['N'] = "彰化縣",
            ['O'] = "新竹市",
            ['P'] = "雲林縣",
            ['Q'] = "嘉義縣",
            ['R'] = "台南縣(已廢止)",
            ['S'] = "高雄縣(已廢止)",
            ['T'] = "屏東縣",
            ['U'] = "花蓮縣",
            ['V'] = "台東縣",
            ['W'] = "金門縣",
            ['X'] = "澎湖縣",
            ['Y'] = "陽明山(舊代碼)",
            ['Z'] = "連江縣"
        };

        private static readonly Dictionary<string, int> _typeMap = new()
        {
            { "工廠", 6 },
            { "公寓", 2 },
            { "公寓(5樓含以下無電梯)", 2 },
            { "住宅大樓", 1 },
            { "住宅大樓(11層含以上有電梯)", 1 },
            { "其他", 7 },
            { "店面", 5 },
            { "店面(店鋪)", 5 },
            { "倉庫", 7 },
            { "套房", 3 },
            { "套房(1房1廳1衛)", 3 },
            { "透天厝", 4 },
            { "華廈", 1 },
            { "華廈(10層含以下有電梯)", 1 },
            { "農舍", 7 },
            { "廠辦", 6 },
            { "辦公商業大樓", 6 }
        };

        public static string? GetCountyFromFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || fileName.Length < 6)
                return null;

            char code = char.ToUpper(fileName[5]);

            return CountyCode.TryGetValue(code, out var county)
                ? county
                : null;
        }

        /// <summary>
        /// 傳入建物型態字串，回傳對應代碼，沒有則回傳 0
        /// </summary>
        public static int GetBuildingTypeCode(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            input = input.Trim();

            foreach (var kv in _typeMap)
            {
                if (input.Contains(kv.Key))
                    return kv.Value;
            }

            return 0; // 找不到
        }
    }
}
