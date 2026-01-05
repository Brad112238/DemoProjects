/****** Object:  Table [dbo].[NetPricePark]    Script Date: 2026/1/5 ¤W¤È 10:30:43 ******/
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

ALTER TABLE [dbo].[NetPricePark]  WITH CHECK ADD  CONSTRAINT [FK_NetPricePark_NetPriceMain] FOREIGN KEY([MainId])
REFERENCES [dbo].[NetPriceMain] ([Id])
GO

ALTER TABLE [dbo].[NetPricePark] CHECK CONSTRAINT [FK_NetPricePark_NetPriceMain]
GO


