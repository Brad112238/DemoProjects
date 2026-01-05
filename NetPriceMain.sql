/****** Object:  Table [dbo].[NetPriceMain]    Script Date: 2026/1/5 ¤W¤È 10:30:21 ******/
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

ALTER TABLE [dbo].[NetPriceMain]  WITH CHECK ADD  CONSTRAINT [FK_NetPriceMain_NetPriceCoordinate] FOREIGN KEY([CoordinateId])
REFERENCES [dbo].[NetPriceCoordinate] ([Id])
GO

ALTER TABLE [dbo].[NetPriceMain] CHECK CONSTRAINT [FK_NetPriceMain_NetPriceCoordinate]
GO


