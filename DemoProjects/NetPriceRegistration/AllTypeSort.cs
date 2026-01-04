using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace NetPriceRegistration
{
    public static class AllTypeSort
    {
        public static void SortType()
        {
            string folder = @"C:\Users";
            string targetColumnName = "建物型態";

            List<string> resultList = [];

            foreach (var file in Directory.GetFiles(folder, "*.xls"))
            {
                Console.WriteLine($"讀取：{Path.GetFileName(file)}");

                try
                {
                    using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workbook = new HSSFWorkbook(fs);
                        var sheet = workbook.GetSheetAt(0);
                        var headerRow = sheet.GetRow(0);

                        int targetColumnIndex = -1;

                        for (int col = 0; col < headerRow.LastCellNum; col++)
                        {
                            var cell = headerRow.GetCell(col);
                            if (cell != null && cell.ToString().Trim() == targetColumnName)
                            {
                                targetColumnIndex = col;
                                break;
                            }
                        }

                        if (targetColumnIndex == -1)
                        {
                            Console.WriteLine($"找不到欄位：{targetColumnName}");
                            continue;
                        }

                        for (int row = 1; row <= sheet.LastRowNum; row++)
                        {
                            try
                            {
                                var dataRow = sheet.GetRow(row);
                                if (dataRow == null) continue;

                                var cell = dataRow.GetCell(targetColumnIndex);
                                if (cell == null) continue;

                                string value = cell.ToString().Trim();
                                if (!string.IsNullOrWhiteSpace(value))
                                    resultList.Add(value);
                            }
                            catch (Exception exRow)
                            {
                                Console.WriteLine($"讀取列 {row} 時發生錯誤：{exRow.Message}");
                                continue;
                            }
                        }
                    }
                }
                catch (Exception exFile)
                {
                    Console.WriteLine($"讀取檔案發生錯誤：{Path.GetFileName(file)}");
                    Console.WriteLine($"錯誤訊息：{exFile.Message}");
                    continue;
                }
            }

            List<string> distinctTypes = [];

            try
            {
                distinctTypes = resultList
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"distinct / 排序過程發生錯誤：{ex.Message}");
            }

            Console.WriteLine("=== 不重複建物型態 ===");
            foreach (var item in distinctTypes)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($"共 {distinctTypes.Count} 種建物型態");
        }

    }
}