using HouseGo.Tools.PriceRegistration.Helper;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System.Text;
using System.Text.RegularExpressions;

namespace NetPriceRegistration
{
    /// <summary>
    /// 處理全部資料
    /// </summary>
    public static class MainWork
    {
        private static HouseGoA10Context CreateDb()
        {
            var options = new DbContextOptionsBuilder<HouseGoA10Context>().UseSqlServer("Server=tcp:fx28acw6op.database.windows.net,1433;Database=HouseGoA10;User ID=VisUser@fx28acw6op;Password=284ru6@*$"+"XU$;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;").Options;

            return new HouseGoA10Context(options);
        }

        /// <summary>
        /// 主程式
        /// </summary>
        public static async Task MainSort()
        {
            string folder = @"C:\Users\心宏\Desktop\實價登錄測試塞資料台北";

            foreach (var file in Directory.GetFiles(folder))
            {
                string fileName = Path.GetFileNameWithoutExtension(file).ToLower();

                if (fileName.Contains("list"))
                {
                    Console.WriteLine($"{fileName}");
                    await ProcessListFile(file, fileName);
                    Console.WriteLine($"========================================================================================");
                }
                else if (fileName.Contains("edit"))
                {
                    Console.WriteLine($"{fileName}");
                    await ProcessEditFile(file);
                    Console.WriteLine($"========================================================================================");
                }
                else
                {
                    Console.WriteLine($"檔名不符合規則，跳過：{fileName}");
                }
            }
        }

        /// <summary>
        /// 處理excel表是list的檔案 代表有資料要新增
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static async Task ProcessListFile(string filePath, string fileName)
        {
            string? city = CityHelper.GetCountyFromFileName(fileName);
            string[] keywords = { "不動產買賣", "建物", "土地", "停車位" };

            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var workbook = new HSSFWorkbook(fs); // XLS

            foreach (var keyword in keywords)
            {
                // 找出所有符合的 sheet
                var sheets = GetSheetsByKeyword(workbook, keyword);

                if (sheets.Count == 0)
                {
                    Console.WriteLine($"沒有找到包含「{keyword}」的工作表：{filePath}");
                    continue;
                }

                foreach (var sheet in sheets)
                {
                    Console.WriteLine($"處理 Sheet：{sheet.SheetName}");

                    var rows = ReadExcelSheet(sheet);

                    switch (keyword)
                    {
                        case "不動產買賣":
                            await RealEstateSales(rows, city, fileName, sheet.SheetName);
                            break;

                        case "建物":
                            await BuildInsert(rows, fileName, sheet.SheetName);
                            break;

                        case "土地":
                            await LandInsert(rows, fileName, sheet.SheetName);
                            break;

                        case "停車位":
                            await ParkingInsert(rows, fileName, sheet.SheetName);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 處理excel表是edit的檔案 代表以前的資料需要修正
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static async Task ProcessEditFile(string filePath)
        {

        }
        
        private static List<ISheet> GetSheetsByKeyword(IWorkbook workbook, string keyword)
        {
            var result = new List<ISheet>();

            for (int i = 0; i < workbook.NumberOfSheets; i++)
            {
                string name = workbook.GetSheetName(i);

                if (name.Contains(keyword))
                {
                    result.Add(workbook.GetSheetAt(i));
                }
            }

            return result;
        }

        public static List<Dictionary<string, object>> ReadExcelSheet(ISheet sheet)
        {
            var result = new List<Dictionary<string, object>>();
            var headerRow = sheet.GetRow(0);
            int columnCount = headerRow.LastCellNum;

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) continue;

                var dict = new Dictionary<string, object>();

                for (int col = 0; col < columnCount; col++)
                {
                    string header = headerRow.GetCell(col)?.ToString()?.Trim();
                    if (string.IsNullOrWhiteSpace(header)) continue;

                    var cell = row.GetCell(col);
                    object value = cell?.ToString()?.Trim();
                    dict[header] = value;
                }

                result.Add(dict);
            }

            return result;
        }

        /// <summary>
        /// 塞入不動產買賣資料至NetPriceMain
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private static async Task RealEstateSales(List<Dictionary<string, object>> rows, string? city, string fileName, string sheetName)
        {
            using var db = CreateDb();
            foreach (var row in rows)
            {
                string? mainId = row.TryGetValue("編號", out var idValue) ? idValue?.ToString() : null;
                try
                {
                    Console.WriteLine(mainId);
                    Console.WriteLine("----------------------------------");
                    string? address = row.TryGetValue("土地位置/建物門牌", out var addressValue) ? addressValue?.ToString() : null;
                    string? area = row.TryGetValue("鄉鎮市區", out var areaValue) ? areaValue?.ToString() : null;
                    string addressFinal = await AddressCut(address, city, area); //要存進資料庫的資料
                    double x = Convert.ToDouble(row["交易標的橫坐標"]);
                    double y = Convert.ToDouble(row["交易標的縱坐標"]);
                    var (lat, lng) = TWD97ToWGS84(x, y);

                    #region 資料整理

                    var coordinateId = await InsertOrGetCoordinate(city, area, address, lng, lat);
                    var tradeType = row.TryGetValue("交易標的", out var tradeTypeValue) ? tradeTypeValue?.ToString() : null;
                    var tradeDate = row.TryGetValue("交易年月日", out var tradeDateValue) ? tradeDateValue?.ToString() : null;
                    var tradeCount = row.TryGetValue("交易筆棟數", out var tradeCountValue) ? tradeCountValue?.ToString() : null;
                    var urbanZone = row.TryGetValue("都市土地使用分區", out var urbanZoneValue) ? urbanZoneValue?.ToString() : null;
                    var nonUrbanZone = row.TryGetValue("非都市土地使用分區", out var nonUrbanZoneValue) ? nonUrbanZoneValue?.ToString() : null;
                    var nonUrbanCategory = row.TryGetValue("非都市土地使用編定", out var nonUrbanCategoryValue) ? nonUrbanCategoryValue?.ToString() : null;
                    double? landTransferArea = row.TryGetValue("土地移轉總面積(平方公尺)", out var v) && double.TryParse(v?.ToString(), out var d) ? Math.Round(d, 2) : null;
                    var buildTypeOrigin = row.TryGetValue("建物型態", out var buildTypeOriginValue) ? buildTypeOriginValue?.ToString() : null;
                    var purpose = row.TryGetValue("主要用途", out var purposeValue) ? purposeValue?.ToString() : null;
                    var material = row.TryGetValue("主要建材", out var materialValue) ? materialValue?.ToString() : null;
                    var buildCompleteDate = row.TryGetValue("建築完成年月", out var buildCompleteDateValue) ? buildCompleteDateValue?.ToString() : null;
                    double? totalArea = row.TryGetValue("建物移轉總面積(平方公尺)", out var ta) &&  double.TryParse(ta?.ToString(), out var taf) ? Math.Round(taf, 2) : null;
                    double? mainArea = row.TryGetValue("主建物面積", out var ma) && double.TryParse(ma?.ToString(), out var maf) ? Math.Round(maf, 2) : null;
                    double? accessoryArea = row.TryGetValue("附屬建物面積", out var aA) && double.TryParse(aA?.ToString(), out var aAf) ? Math.Round(aAf, 2) : null;
                    double? balconyArea = row.TryGetValue("陽台面積", out var bA) && double.TryParse(bA?.ToString(), out var bAf) ? Math.Round(bAf, 2) : null;
                    int? room = row.TryGetValue("建物現況格局-房", out var r) && int.TryParse(r?.ToString(), out var rf) ? rf : (int?)null;
                    int? hall = row.TryGetValue("建物現況格局-廳", out var h) && int.TryParse(h?.ToString(), out var hf) ? hf : (int?)null;
                    int? bath = row.TryGetValue("建物現況格局-衛", out var b) && int.TryParse(b?.ToString(), out var bf) ? bf : (int?)null;
                    var houseCompartmnet = row.TryGetValue("建物現況格局-隔間", out var houseCompartmnetValue) ? houseCompartmnetValue?.ToString() : null;
                    var elevator = row.TryGetValue("電梯", out var elevatorValue) ? elevatorValue?.ToString() : null;
                    var hasManagement = row.TryGetValue("有無管理組織", out var hasManagementValue) ? hasManagementValue?.ToString() : null;
                    int? totalPrice = row.TryGetValue("總價(元)", out var tP) && int.TryParse(tP.ToString(), out var tPf) ? tPf : (int?)null;
                    int? unitPrice = row.TryGetValue("單價(元/平方公尺)", out var uP) && int.TryParse(uP.ToString(), out var uPf) ? uPf : (int?)null;
                    var parkinType = row.TryGetValue("車位類別", out var parkinTypeValue) ? parkinTypeValue?.ToString() : null;
                    var remark = row.TryGetValue("備註", out var remarkValue) ? remarkValue?.ToString() : null;
                    double? parkingArea = row.TryGetValue("車位移轉總面積(平方公尺)", out var pA) && double.TryParse(pA?.ToString(), out var pAf) ? Math.Round(pAf, 2) : null;
                    int? parkingPrice = row.TryGetValue("車位總價(元)", out var pP) && int.TryParse(pP.ToString(), out var pPf) ? pPf : (int?)null;
                    bool specialTrans = await SpecialTrans(remark);

                    #endregion

                    #region 樓層處理
                    var floorSEOrigin = row.TryGetValue("移轉層次", out var floorSEOriginValue) ? floorSEOriginValue?.ToString() : null;
                    var floorCountOrigin = row.TryGetValue("總樓層數", out var floorCountOriginValue) ? floorCountOriginValue?.ToString() : null;
                    //floorSEOrigin = "一層、三層、地下二層、地下五層、電梯樓梯間、平台"; // 測試用
                    //floorSEOrigin = "全"; // 測試用
                    var floorCount = await ParseSingleFloorAsync(floorCountOrigin);
                    var (floorS, floorE) = await ParseFloorRangeAsync(floorSEOrigin);
                    if (floorSEOrigin == "全")
                    {
                        floorS = 1;
                        floorE = floorCount;
                    }               
                    #endregion

                    var netPriceMain = new NetPriceMain
                    {
                        CoordinateId = coordinateId,
                        Oid = mainId ?? "",
                        City = city ?? "",
                        Area = area ?? "",
                        FullAddress = address != null ? await AddressReplaceTrad(address) : "",
                        Address = addressFinal,
                        TradeType = tradeType ?? "",
                        TradeDate = tradeDate ?? "",
                        TradeCount = tradeCount ?? "",
                        UrbanZone = urbanZone == "" ? null : urbanZone,
                        NonUrbanZone = nonUrbanZone == "" ? null : nonUrbanZone,
                        NonUrbanCategory = nonUrbanCategory == "" ? null : nonUrbanCategory,
                        LandTransferArea = landTransferArea ?? 0,
                        BuildTypeOrigin = buildTypeOrigin ?? "",
                        BuildType = tradeType == "土地" ? 8 : CityHelper.GetBuildingTypeCode(buildTypeOrigin),
                        FloorS = floorS,
                        FloorE = floorE,
                        FloorCount = floorCount,
                        FloorOrigin = floorSEOrigin == "" ? null : floorSEOrigin,
                        FloorCountOrigin = floorCountOrigin == "" ? null : floorCountOrigin,
                        Purpose = purpose == "" ? null : purpose,
                        Material = material == "" ? null : material,
                        BuildCompleteDate = buildCompleteDate == "" ? null : buildCompleteDate,
                        TotalArea = totalArea ?? 0,
                        MainArea = mainArea,
                        AccessoryArea = accessoryArea,
                        BalconyArea = balconyArea,
                        Room = room,
                        Hall = hall,
                        Bath = bath,
                        HouseCompartment = houseCompartmnet == "有",
                        Elevator = elevator == "有",
                        HasManagemnet = hasManagement == "有",
                        TotalPrice = totalPrice ?? 0,
                        UnitPrice = unitPrice,
                        ParkingType = parkinType == "" ? null : parkinType,
                        CreateTime = DateTime.Now,
                        Latitude = lat,
                        Longitude = lng,
                        Remark = remark == "" ? null : remark,
                        ParkingArea = parkingArea,
                        ParkingPrice = parkingPrice,
                        SpecialTrans = specialTrans
                    };

                    await db.AddAsync(netPriceMain);
                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    await WriteLog($"{fileName}{sheetName}", mainId, e.Message.ToString());
                    continue;
                }
            }
        }

        /// <summary>
        /// 座標轉換公式
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static (double lat, double lng) TWD97ToWGS84(double x, double y)
        {
            var csFactory = new CoordinateSystemFactory();
            var ctFactory = new CoordinateTransformationFactory();

            // TWD97 TM2 (121E)
            var twd97 = csFactory.CreateFromWkt(@"
                PROJCS[""TWD97 / TM2"",
                    GEOGCS[""TWD97"",
                        DATUM[""TWD97"",
                            SPHEROID[""GRS 1980"",6378137,298.257222101]],
                        PRIMEM[""Greenwich"",0],
                        UNIT[""degree"",0.0174532925199433]],
                    PROJECTION[""Transverse_Mercator""],
                    PARAMETER[""latitude_of_origin"",0],
                    PARAMETER[""central_meridian"",121],
                    PARAMETER[""scale_factor"",0.9999],
                    PARAMETER[""false_easting"",250000],
                    PARAMETER[""false_northing"",0],
                    UNIT[""metre"",1]]");

            var wgs84 = GeographicCoordinateSystem.WGS84;

            var transform = ctFactory.CreateFromCoordinateSystems(twd97, wgs84);

            var result = transform.MathTransform.Transform(new[] { x, y });

            return (result[1], result[0]); // lat, lng
        }

        /// <summary>
        /// 資料塞到座標表裡面NetPriceCoordinate
        /// </summary>
        /// <param name="mainId"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        private static async Task<int> InsertOrGetCoordinate(string? city, string? area, string? address, double? longitude, double? latitude)
        {
            try
            {
                using var db = CreateDb();
                var cities = new[] { "臺北", "臺中", "臺南", "臺東" };
                if (address.Contains(city) || cities.Any(c => address.Contains(c))) //這是建物 要切號
                {
                    var addressOnlyNo = await CutUntilFirstHouseNo(address);

                    addressOnlyNo = await AddressReplaceTrad(addressOnlyNo);
                    addressOnlyNo = addressOnlyNo.Replace(city, "");
                    addressOnlyNo = addressOnlyNo.Replace(area, "");

                    var addressNoInDbId = await db.NetPriceCoordinates.Where(x => x.City == city && x.Area == area && x.Address == addressOnlyNo).Select(y => y.Id).FirstOrDefaultAsync();
                    if (addressNoInDbId == 0)
                    {
                        var entity = new NetPriceCoordinate
                        {
                            City = city,
                            Area = area,
                            Address = addressOnlyNo,
                            CreateTime = DateTime.Now,
                            Latitude = (double)latitude,
                            Longitude = (double)longitude
                        };

                        await db.AddAsync(entity);
                        await db.SaveChangesAsync();
                        return entity.Id;
                    }
                    return addressNoInDbId;
                }
                else
                {
                    var sectionInDbId = await db.NetPriceCoordinates.Where(x => x.City == city && x.Area == area && x.Address == address).Select(y => y.Id).FirstOrDefaultAsync();
                    if (sectionInDbId == 0)
                    {
                        var entity = new NetPriceCoordinate
                        {
                            City = city,
                            Area = area,
                            Address = address,
                            CreateTime = DateTime.Now,
                            Latitude = (double)latitude,
                            Longitude = (double)longitude
                        };

                        await db.AddAsync(entity);
                        await db.SaveChangesAsync();
                        return entity.Id;
                    }
                    return sectionInDbId;
                }
            }
            catch (Exception ex) 
            {
                return 0;
            }
        }

        /// <summary>
        /// 建物表新增
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private static async Task BuildInsert(List<Dictionary<string, object>> rows, string fileName, string sheetName)
        {
            using var db = CreateDb();
            Console.WriteLine($"{fileName}進入{sheetName}");
            foreach (var row in rows)
            {
                string? mainId = row.TryGetValue("編號", out var idValue) ? idValue?.ToString() : null;
                try
                {
                    string? purpose = row.TryGetValue("主要用途", out var purposeValue) ? purposeValue?.ToString() : null;
                    string? material = row.TryGetValue("主要建材", out var materialValue) ? materialValue?.ToString() : null;
                    string? buildCompleteDate = row.TryGetValue("建築完成日期", out var buildCompleteDateValue) ? buildCompleteDateValue?.ToString() : null;
                    string? floorCount = row.TryGetValue("總層數", out var floorCountDateValue) ? floorCountDateValue?.ToString() : null;
                    string? floorInfo = row.TryGetValue("建物分層", out var floorInfoValue) ? floorInfoValue?.ToString() : null;
                    string? transferType = row.TryGetValue("移轉情形", out var transferTypeValue) ? transferTypeValue?.ToString() : null;
                    int? houseYear = row.TryGetValue("屋齡", out var hY) && int.TryParse(hY.ToString(), out var hYf) ? hYf : (int?)null;
                    double? totalArea = row.TryGetValue("建物移轉面積平方公尺", out var ta) && double.TryParse(ta?.ToString(), out var taf) ? Math.Round(taf, 2) : null;

                    var netPriceMain = await db.NetPriceMains.Where(x => x.Oid == mainId).FirstOrDefaultAsync();

                    if (netPriceMain != null)
                    {
                        if (netPriceMain.HouseAge == null)
                        {
                            netPriceMain.HouseAge = houseYear;
                        }

                        var netPriceBuild = new NetPriceBuild
                        {
                            MainId = netPriceMain.Id,
                            CreateTime = DateTime.Now,
                            MainPurpose = purpose,
                            Material = material,
                            BuildCompleteDate = buildCompleteDate,
                            FloorCount = floorCount,
                            FloorInfo = floorInfo,
                            TransferType = transferType,
                            HouseYear = houseYear,
                            TotalArea = totalArea ?? 0,
                        };

                        await db.AddAsync(netPriceBuild);
                        await db.SaveChangesAsync();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    await WriteLog($"{fileName}{sheetName}", mainId, e.Message.ToString());
                }
            }
        }      

        /// <summary>
        /// 土地表新增
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private static async Task LandInsert(List<Dictionary<string, object>> rows, string fileName, string sheetName)
        {
            using var db = CreateDb();
            Console.WriteLine($"{fileName}進入{sheetName}");
            foreach (var row in rows)
            {
                string? mainId = row.TryGetValue("編號", out var idValue) ? idValue?.ToString() : null;
                try
                {
                    string? section = row.TryGetValue("土地位置", out var sectionValue) ? sectionValue?.ToString() : null;
                    string? landNo = row.TryGetValue("地號", out var landNoValue) ? landNoValue?.ToString() : null;
                    string? landUseType = row.TryGetValue("使用分區或編定", out var buildCompleteDateValue) ? buildCompleteDateValue?.ToString() : null;
                    string? transferType = row.TryGetValue("移轉情形", out var transferTypeValue) ? transferTypeValue?.ToString() : null;
                    int? shareDenominator = row.TryGetValue("權利人持分分母", out var sD) && int.TryParse(sD.ToString(), out var sDf) ? sDf : (int?)0;
                    int? shareNumerator = row.TryGetValue("權利人持分分子", out var sH) && int.TryParse(sH.ToString(), out var sHf) ? sHf : (int?)0;
                    double? totalArea = row.TryGetValue("土地移轉面積平方公尺", out var ta) && double.TryParse(ta?.ToString(), out var taf) ? Math.Round(taf, 2) : 0;

                    var netPriceMainId = await db.NetPriceMains.Where(x => x.Oid == mainId).Select(y => y.Id).FirstOrDefaultAsync();
                    var netPriceland = new NetPriceLand
                    {
                        CreateTime = DateTime.Now,
                        Section = section ?? "",
                        LandNo = landNo ?? "",
                        TotalArea = totalArea ?? 0,
                        LandUseType = landUseType ?? "",
                        TransferType = transferType ?? "",
                        MainId = netPriceMainId,
                        ShareDenominator = (int)shareDenominator,
                        ShareNumerator = (int)shareNumerator
                    };

                    await db.AddAsync(netPriceland);
                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    await WriteLog($"{fileName}{sheetName}", mainId, e.Message.ToString());
                }
            }
        }

        /// <summary>
        /// 車位表新增
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private static async Task ParkingInsert(List<Dictionary<string, object>> rows, string fileName, string sheetName)
        {
            using var db = CreateDb();
            Console.WriteLine($"{fileName}進入{sheetName}");
            foreach (var row in rows)
            {
                string? mainId = row.TryGetValue("編號", out var idValue) ? idValue?.ToString() : null;
                try
                {
                    string? parkingType = row.TryGetValue("車位類別", out var parkingTypeValue) ? parkingTypeValue?.ToString() : null;
                    string? parkingFloor = row.TryGetValue("車位所在樓層", out var parkingFloorValue) ? parkingFloorValue?.ToString() : null;        
                    int? totalPrice = row.TryGetValue("權利人持分分子", out var sH) && int.TryParse(sH.ToString(), out var sHf) ? sHf : (int?)0;
                    double? totalArea = row.TryGetValue("車位面積平方公尺", out var ta) && double.TryParse(ta?.ToString(), out var taf) ? Math.Round(taf, 2) : 0;

                    var netPriceMainId = await db.NetPriceMains.Where(x => x.Oid == mainId).Select(y => y.Id).FirstOrDefaultAsync();
                    var netPricepark = new NetPricePark
                    {
                        CreateTime = DateTime.Now,
                        MainId = netPriceMainId,
                        ParkingType = parkingType ?? "",
                        ParkingFloor = parkingFloor ?? "",
                        TotalArea = totalArea ?? 0,
                        TotalPrice = totalPrice ?? 0
                    };

                    await db.AddAsync(netPricepark);
                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    await WriteLog($"{fileName}{sheetName}", mainId, e.Message.ToString());
                }
            }
        }

        private static async Task<string> AddressCut(string? address, string? city, string? area)
        {
            if (string.IsNullOrWhiteSpace(address))
                return address;

            address = address.Replace(city ?? "", "", StringComparison.Ordinal);
            address = address.Replace(area ?? "", "", StringComparison.Ordinal);

            List<string> taiTraditional = [ "臺北市", "臺中市", "臺南市", "臺東市", "台北縣", "台中縣", "台南縣", "台東縣" ];

            foreach (var word in taiTraditional)
            {
                if (address.Contains(word))
                {
                    address = address.Replace(word, "");
                    break;
                }
            }

            return address.Trim();
        }

        private static async Task<string> AddressReplaceTrad(string address)
        {
            if (address.Contains("臺北市"))
            {
                address = address.Replace("臺北市", "台北市");
            }
            else if (address.Contains("臺中市"))
            {
                address = address.Replace("臺中市", "台中市");
            }
            else if (address.Contains("臺南市"))
            {
                address = address.Replace("臺南市", "台南市");
            }
            else if (address.Contains("臺東市"))
            {
                address = address.Replace("臺東市", "台東市");
            }

            return address.Trim();
        }

        private static async Task<string>CutUntilFirstHouseNo(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return string.Empty;

            // 找第一個「號」
            int idx = address.IndexOf("號");
            if (idx == -1)
                return address;

            // 保留到「號」結束
            return address.Substring(0, idx + 1);
        }

        private static async Task<int?> ParseSingleFloorAsync(string input)
        {
            return await Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(input))
                    return (int?)null;

                string part = input.Trim();

                // 必須包含「層」或「樓」
                if (!part.Contains("層") && !part.Contains("樓"))
                    return (int?)null;

                bool isBasement = part.Contains("地下");

                // 擷取中文數字
                var match = Regex.Match(part, @"[一二三四五六七八九十百零〇]+");
                if (!match.Success)
                    return (int?)null;

                int number = ChineseNumberToInt(match.Value);
                if (number == 0)
                    return (int?)null;

                if (isBasement)
                    number = -number;

                return (int?)number;
            });
        }

        private static Task<(int? floorS, int? floorE)> ParseFloorRangeAsync(string input)
        {
            return Task.Run<(int? floorS, int? floorE)>(() =>
            {
                if (string.IsNullOrWhiteSpace(input))
                    return (null, null);

                var parts = input.Split(new[] { '、', ',', '，', '・' }, StringSplitOptions.RemoveEmptyEntries);

                List<int> floors = new();

                foreach (var raw in parts)
                {
                    var part = raw.Trim();

                    if (!part.Contains("層"))
                        continue;

                    bool isBasement = part.Contains("地下");

                    var match = Regex.Match(part, @"[一二三四五六七八九十百零〇]+");
                    if (!match.Success)
                        continue;

                    int number = ChineseNumberToInt(match.Value);

                    if (number == 0)
                        continue;

                    if (isBasement)
                        number = -number;

                    floors.Add(number);
                }

                if (floors.Count == 0)
                    return (null, null);

                return (floors.Min(), floors.Max());
            });
        }

        private static int ChineseNumberToInt(string chinese)
        {
            Dictionary<char, int> map = new Dictionary<char, int>
            {
                {'零', 0}, {'〇', 0},
                {'一', 1}, {'二', 2}, {'三', 3}, {'四', 4}, {'五', 5},
                {'六', 6}, {'七', 7}, {'八', 8}, {'九', 9},
                {'十', 10}, {'百', 100}
            };

            int result = 0;
            int temp = 0;

            foreach (var c in chinese)
            {
                if (!map.ContainsKey(c)) continue;

                int value = map[c];

                if (value == 10 || value == 100)
                {
                    if (temp == 0) temp = 1;
                    temp *= value;
                    result += temp;
                    temp = 0;
                }
                else
                {
                    temp = value;
                }
            }

            result += temp;
            return result;
        }

        private static async Task<bool> SpecialTrans(string? remark)
        {
            if (remark == null)
            {
                return false;
            }
            var keywords = new List<string> { "夫妻", "特殊關係", "政府機關", "配偶", "預售屋", "夫妻" };
            foreach (var key in keywords)
            {
                if (remark.Contains(key))
                    return true;
            }
            return false;
        }

        private static async Task WriteLog(string fileName, string mainId, string message)
        {
            string folder = @"C:";

            try
            {
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fullPath = Path.Combine(folder, $"{fileName}哪些資料有例外.txt");

                string log = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | MainId = {mainId} | {message}";
                File.AppendAllText(fullPath, log + Environment.NewLine, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Log 寫入失敗：" + ex.Message);
            }
        }
    }
}
