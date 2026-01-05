/****** Object:  Table [dbo].[NetPriceBuild]    Script Date: 2026/1/5 上午 10:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NetPriceBuild](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MainId] [int] NOT NULL,
	[HouseYear] [int] NULL,
	[TotalArea] [float] NOT NULL,
	[MainPurpose] [nvarchar](max) NULL,
	[Material] [nvarchar](50) NULL,
	[BuildCompleteDate] [nvarchar](20) NULL,
	[FloorCount] [nvarchar](20) NULL,
	[FloorInfo] [nvarchar](200) NULL,
	[TransferType] [nvarchar](50) NULL,
	[CreateTime] [datetime] NOT NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_NetPriceBuild] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NetPriceCoordinate]    Script Date: 2026/1/5 上午 10:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NetPriceCoordinate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Longitude] [float] NOT NULL,
	[Latitude] [float] NOT NULL,
	[City] [nvarchar](20) NOT NULL,
	[Area] [nvarchar](20) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_NetPriceCoordinate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NetPriceLand]    Script Date: 2026/1/5 上午 10:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NetPriceLand](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MainId] [int] NOT NULL,
	[Section] [nvarchar](50) NOT NULL,
	[TotalArea] [float] NOT NULL,
	[LandUseType] [nvarchar](50) NOT NULL,
	[TransferType] [nvarchar](50) NOT NULL,
	[LandNo] [nvarchar](20) NOT NULL,
	[ShareDenominator] [int] NOT NULL,
	[ShareNumerator] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_NetPriceLand] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NetPriceMain]    Script Date: 2026/1/5 上午 10:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NetPriceMain](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CoordinateId] [int] NOT NULL,
	[Oid] [nvarchar](50) NOT NULL,
	[City] [nvarchar](20) NOT NULL,
	[Area] [nvarchar](20) NOT NULL,
	[FullAddress] [nvarchar](max) NOT NULL,
	[Address] [nvarchar](300) NOT NULL,
	[TradeType] [nvarchar](20) NOT NULL,
	[TradeDate] [nvarchar](20) NOT NULL,
	[TradeCount] [nvarchar](30) NOT NULL,
	[UrbanZone] [nvarchar](50) NULL,
	[NonUrbanZone] [nvarchar](50) NULL,
	[NonUrbanCategory] [nvarchar](50) NULL,
	[LandTransferArea] [float] NOT NULL,
	[BuildTypeOrigin] [nvarchar](50) NOT NULL,
	[BuildType] [int] NOT NULL,
	[FloorS] [int] NULL,
	[FloorE] [int] NULL,
	[FloorCount] [int] NULL,
	[FloorOrigin] [nvarchar](200) NULL,
	[FloorCountOrigin] [nvarchar](20) NULL,
	[Purpose] [nvarchar](max) NULL,
	[Material] [nvarchar](50) NULL,
	[BuildCompleteDate] [nvarchar](20) NULL,
	[TotalArea] [float] NOT NULL,
	[MainArea] [float] NULL,
	[AccessoryArea] [float] NULL,
	[BalconyArea] [float] NULL,
	[Room] [int] NULL,
	[Hall] [int] NULL,
	[Bath] [int] NULL,
	[HouseCompartment] [bit] NOT NULL,
	[Elevator] [bit] NOT NULL,
	[HasManagemnet] [bit] NOT NULL,
	[TotalPrice] [int] NOT NULL,
	[UnitPrice] [int] NULL,
	[ParkingType] [nvarchar](20) NULL,
	[ParkingArea] [float] NULL,
	[ParkingPrice] [int] NULL,
	[HouseAge] [int] NULL,
	[Community] [nvarchar](50) NULL,
	[Longitude] [float] NOT NULL,
	[Latitude] [float] NOT NULL,
	[SpecialTrans] [bit] NOT NULL,
	[Remark] [nvarchar](max) NULL,
	[CreateTime] [datetime] NOT NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_NetPriceMain] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NetPricePark]    Script Date: 2026/1/5 上午 10:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NetPricePark](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MainId] [int] NOT NULL,
	[ParkingType] [nvarchar](50) NOT NULL,
	[TotalPrice] [int] NOT NULL,
	[TotalArea] [float] NOT NULL,
	[ParkingFloor] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[ModifyTime] [datetime] NULL,
 CONSTRAINT [PK_NetPricePark] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObjHistory]    Script Date: 2026/1/5 上午 10:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObjHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MainId] [int] NOT NULL,
	[CId] [int] NOT NULL,
	[Vision] [int] NULL,
	[OId] [nvarchar](40) NULL,
	[City] [nvarchar](10) NOT NULL,
	[Area] [nvarchar](10) NOT NULL,
	[Street] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](50) NULL,
	[CaseName] [nvarchar](100) NULL,
	[TotalArea] [float] NULL,
	[MainArea] [float] NULL,
	[OfferDate] [datetime] NOT NULL,
	[TotalPrice] [float] NULL,
	[AgencyPageUrl] [nvarchar](200) NULL,
	[ImgUrl] [nvarchar](1000) NULL,
	[FloorFrom] [int] NULL,
	[FloorTo] [int] NULL,
	[FloorCount] [int] NULL,
	[AgencyCompanyName] [nvarchar](100) NULL,
	[AgencyName] [nvarchar](100) NULL,
	[AgencyStoreName] [nvarchar](100) NULL,
	[AgencySalerName] [nvarchar](20) NULL,
	[AgencyPhone] [nvarchar](30) NULL,
	[AgencyMobile] [nvarchar](30) NULL,
 CONSTRAINT [PK_ObjHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObjMain]    Script Date: 2026/1/5 上午 10:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObjMain](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CId] [int] NULL,
	[City] [nvarchar](10) NULL,
	[Area] [nvarchar](10) NULL,
	[Street] [nvarchar](20) NULL,
	[Address] [nvarchar](50) NULL,
	[TotalArea] [float] NULL,
	[MainArea] [float] NULL,
	[FloorFrom] [int] NULL,
	[FloorCount] [int] NULL,
	[HouseYY] [int] NOT NULL,
	[Longitude] [float] NULL,
	[Latitude] [float] NULL,
	[OnLine] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_ObjMain] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObjOnLine]    Script Date: 2026/1/5 上午 10:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObjOnLine](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MainId] [int] NOT NULL,
	[CId] [int] NULL,
	[Vision] [int] NULL,
	[OId] [nvarchar](40) NOT NULL,
	[City] [nvarchar](10) NOT NULL,
	[Area] [nvarchar](10) NOT NULL,
	[Street] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](50) NULL,
	[CaseName] [nvarchar](100) NULL,
	[Community] [nvarchar](50) NULL,
	[AgencyPageUrl] [nvarchar](200) NULL,
	[ImgUrl] [nvarchar](1000) NULL,
	[TotalArea] [float] NULL,
	[MainArea] [float] NULL,
	[LandArea] [float] NULL,
	[PublicArea] [float] NULL,
	[AttachArea] [float] NULL,
	[HouseYear] [float] NULL,
	[BuildType] [nvarchar](20) NULL,
	[TotalPrice] [float] NULL,
	[UnitPrice] [float] NULL,
	[RoomCount] [float] NULL,
	[HallCount] [float] NULL,
	[BathroomCount] [float] NULL,
	[DenCount] [float] NULL,
	[FloorFrom] [int] NULL,
	[FloorTo] [int] NULL,
	[FloorCount] [int] NULL,
	[Parking] [int] NULL,
	[Longitude] [float] NULL,
	[Latitude] [float] NULL,
	[CreateDate] [datetime] NOT NULL,
	[RefreshDate] [datetime] NOT NULL,
	[LastRefreshDetailDate] [datetime] NULL,
	[StrikeDate] [datetime] NULL,
	[DownDate] [datetime] NULL,
	[Online] [int] NULL,
	[DetailId] [nvarchar](30) NULL,
	[HouseType] [nvarchar](40) NULL,
	[ParkingType] [nvarchar](30) NULL,
	[HousePurpose] [nvarchar](20) NULL,
	[HouseArchitecture] [nvarchar](50) NULL,
	[HouseDirection] [nvarchar](20) NULL,
	[HouseMaterial] [nvarchar](30) NULL,
	[Layout] [nvarchar](50) NULL,
	[ElevatorNum] [nvarchar](10) NULL,
	[SideRoom] [nvarchar](20) NULL,
	[DarkRoom] [nvarchar](20) NULL,
	[Courtyard] [nvarchar](20) NULL,
	[Alley] [nvarchar](10) NULL,
	[ManagementType] [nvarchar](20) NULL,
	[MonthlyPayGuard] [nvarchar](30) NULL,
	[ManagementFee] [nvarchar](30) NULL,
	[MonthlyPI] [int] NULL,
	[CarPrice] [float] NULL,
	[NearByPark] [nvarchar](100) NULL,
	[NearBySchool] [nvarchar](100) NULL,
	[NearByMarket] [nvarchar](100) NULL,
	[Transportation] [nvarchar](200) NULL,
	[Feature1] [nvarchar](1000) NULL,
	[Feature2] [nvarchar](200) NULL,
	[Feature3] [nvarchar](200) NULL,
	[Feature4] [nvarchar](200) NULL,
	[AgencyCompanyName] [nvarchar](100) NULL,
	[AgencyName] [nvarchar](100) NULL,
	[AgencyStoreName] [nvarchar](100) NULL,
	[AgencySalerName] [nvarchar](20) NULL,
	[AgencyStoreAddress] [nvarchar](100) NULL,
	[AgencyPhone] [nvarchar](30) NULL,
	[AgencyMobile] [nvarchar](30) NULL,
	[AgencyEMail] [nvarchar](50) NULL,
	[AgencyLicense] [nvarchar](20) NULL,
 CONSTRAINT [PK_ObjOnLine] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tmp]    Script Date: 2026/1/5 上午 10:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tmp](
	[SeqNo] [int] IDENTITY(1,1) NOT NULL,
	[OId] [nvarchar](30) NOT NULL,
	[CId] [int] NULL,
	[Vision] [int] NULL,
	[CaseName] [nvarchar](100) NULL,
	[AgencyPageUrl] [nvarchar](200) NULL,
	[ImgUrl] [nvarchar](1000) NULL,
	[BuildingType] [nvarchar](10) NULL,
	[City] [nvarchar](10) NOT NULL,
	[Area] [nvarchar](10) NOT NULL,
	[Street] [nvarchar](20) NULL,
	[Address] [nvarchar](50) NULL,
	[TotalAreaSize] [float] NULL,
	[MainAreaSize] [float] NULL,
	[RoomCount] [float] NULL,
	[HallCount] [float] NULL,
	[BathroomCount] [float] NULL,
	[DenCount] [float] NULL,
	[TargetFloorNumberFrom] [int] NULL,
	[TargetFloorNumberTo] [int] NULL,
	[HouseFloorCount] [int] NULL,
	[Parking] [bit] NULL,
	[HouseYear] [float] NULL,
	[Longitude] [float] NULL,
	[Latitude] [float] NULL,
	[TotalPrice] [float] NULL,
	[Community] [nvarchar](20) NULL,
	[Online] [int] NULL,
	[CreateDateTime] [datetime] NULL,
	[RefreshDateTime] [datetime] NULL,
	[LastRefreshDetail] [datetime] NULL,
	[StrikeDateTime] [datetime] NULL,
	[DownDate] [datetime] NULL,
	[DetailId] [nvarchar](30) NULL,
	[SinglePrice] [float] NULL,
	[MonthlyPI] [int] NULL,
	[LandAreaSize] [float] NULL,
	[PublicAreaSize] [float] NULL,
	[AttachSize] [float] NULL,
	[HousePurpose] [nvarchar](20) NULL,
	[HouseType] [nvarchar](20) NULL,
	[HouseArchitecture] [nvarchar](50) NULL,
	[Layout] [nvarchar](50) NULL,
	[HouseDirection] [nvarchar](20) NULL,
	[HouseMaterial] [nvarchar](30) NULL,
	[SideRoom] [nvarchar](20) NULL,
	[DarkRoom] [nvarchar](20) NULL,
	[Courtyard] [nvarchar](20) NULL,
	[ManagementType] [nvarchar](20) NULL,
	[MonthlyPayGuard] [nvarchar](30) NULL,
	[ManagementFee] [nvarchar](30) NULL,
	[Alley] [nvarchar](10) NULL,
	[ElevatorNum] [nvarchar](10) NULL,
	[NearByPark] [nvarchar](100) NULL,
	[NearBySchool] [nvarchar](100) NULL,
	[NearByMarket] [nvarchar](100) NULL,
	[Transportation] [nvarchar](200) NULL,
	[Feature1] [nvarchar](1000) NULL,
	[Feature2] [nvarchar](200) NULL,
	[Feature3] [nvarchar](200) NULL,
	[Feature4] [nvarchar](200) NULL,
	[AgencyCompanyName] [nvarchar](100) NULL,
	[AgencyName] [nvarchar](100) NULL,
	[AgencyStoreName] [nvarchar](100) NULL,
	[AgencySalerName] [nvarchar](20) NULL,
	[AgencyStoreAddress] [nvarchar](100) NULL,
	[AgencyPhone] [nvarchar](30) NULL,
	[AgencyMobile] [nvarchar](30) NULL,
	[AgencyEMail] [nvarchar](50) NULL,
	[AgencyLicense] [nvarchar](20) NULL,
	[CarPrice] [float] NULL,
	[BatchId] [nvarchar](20) NULL,
	[Process] [bit] NULL,
	[ExpFlag] [bit] NULL,
	[ParkingType] [nvarchar](30) NULL,
 CONSTRAINT [PK_Tmp] PRIMARY KEY CLUSTERED 
(
	[SeqNo] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tmp591]    Script Date: 2026/1/5 上午 10:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tmp591](
	[SeqNo] [int] IDENTITY(1,1) NOT NULL,
	[OId] [nvarchar](30) NOT NULL,
	[CId] [int] NULL,
	[Vision] [int] NULL,
	[CaseName] [nvarchar](100) NULL,
	[AgencyPageUrl] [nvarchar](200) NULL,
	[ImgUrl] [nvarchar](1000) NULL,
	[BuildingType] [nvarchar](10) NULL,
	[City] [nvarchar](10) NOT NULL,
	[Area] [nvarchar](10) NOT NULL,
	[Street] [nvarchar](20) NULL,
	[Address] [nvarchar](50) NULL,
	[Image1] [nvarchar](max) NULL,
	[MainAreaSize] [float] NULL,
	[RoomCount] [float] NULL,
	[HallCount] [float] NULL,
	[BathroomCount] [float] NULL,
	[TargetFloorNumberFrom] [int] NULL,
	[TargetFloorNumberTo] [int] NULL,
	[HouseFloorCount] [int] NULL,
	[Parking] [bit] NULL,
	[HouseYear] [float] NULL,
	[TotalPrice] [float] NULL,
	[Community] [nvarchar](20) NULL,
	[Online] [int] NULL,
	[CreateDateTime] [datetime] NULL,
	[RefreshDateTime] [datetime] NULL,
	[LastRefreshDetail] [datetime] NULL,
	[Image2] [nvarchar](max) NULL,
	[HouseType] [nvarchar](20) NULL,
	[AgencySalerName] [nvarchar](20) NULL,
	[BatchId] [nvarchar](20) NULL,
	[Process] [bit] NULL,
	[ExpFlag] [bit] NULL,
	[Image3] [nvarchar](max) NULL,
	[TotalAreaSize] [float] NULL,
 CONSTRAINT [PK_Tmp591] PRIMARY KEY CLUSTERED 
(
	[SeqNo] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[NetPriceBuild]  WITH CHECK ADD  CONSTRAINT [FK_NetPriceBuild_NetPriceMain] FOREIGN KEY([MainId])
REFERENCES [dbo].[NetPriceMain] ([Id])
GO
ALTER TABLE [dbo].[NetPriceBuild] CHECK CONSTRAINT [FK_NetPriceBuild_NetPriceMain]
GO
ALTER TABLE [dbo].[NetPriceLand]  WITH CHECK ADD  CONSTRAINT [FK_NetPriceLand_NetPriceMain] FOREIGN KEY([MainId])
REFERENCES [dbo].[NetPriceMain] ([Id])
GO
ALTER TABLE [dbo].[NetPriceLand] CHECK CONSTRAINT [FK_NetPriceLand_NetPriceMain]
GO
ALTER TABLE [dbo].[NetPriceMain]  WITH CHECK ADD  CONSTRAINT [FK_NetPriceMain_NetPriceCoordinate] FOREIGN KEY([CoordinateId])
REFERENCES [dbo].[NetPriceCoordinate] ([Id])
GO
ALTER TABLE [dbo].[NetPriceMain] CHECK CONSTRAINT [FK_NetPriceMain_NetPriceCoordinate]
GO
ALTER TABLE [dbo].[NetPricePark]  WITH CHECK ADD  CONSTRAINT [FK_NetPricePark_NetPriceMain] FOREIGN KEY([MainId])
REFERENCES [dbo].[NetPriceMain] ([Id])
GO
ALTER TABLE [dbo].[NetPricePark] CHECK CONSTRAINT [FK_NetPricePark_NetPriceMain]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物件網頁原編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'OId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌系統定義代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'CId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌代碼及屋主自售代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'Vision'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物件標題名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'CaseName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物件網址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'AgencyPageUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物件預設圖片網址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'ImgUrl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統物件類別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'BuildingType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地址縣市名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'City'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地址行政區名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'Area'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地址路名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'Street'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地址資訊' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'Address'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建物/土地總坪數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'TotalAreaSize'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建物總坪數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'MainAreaSize'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建物-房間數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'RoomCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建物-廳間數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'HallCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建物-衛間數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'BathroomCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建物-室(書房)間數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'DenCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'起始樓層' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'TargetFloorNumberFrom'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'迄止樓層' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'TargetFloorNumberTo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'建物總樓層' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'HouseFloorCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有車位旗標' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'Parking'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'屋齡' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'HouseYear'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'經度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'Longitude'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'緯度' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'Latitude'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總價' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'TotalPrice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'社區名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'Community'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否線上' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'Online'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增資料時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'CreateDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'RefreshDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物件最後檢查時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'LastRefreshDetail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後一次價格異動時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'StrikeDateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物件下架時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'DownDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物件內頁編碼(與OID 不同時)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp', @level2type=N'COLUMN',@level2name=N'DetailId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單價' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tmp'
GO
