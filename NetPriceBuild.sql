/****** Object:  Table [dbo].[NetPriceBuild]    Script Date: 2026/1/5 ¤W¤È 10:26:55 ******/
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

ALTER TABLE [dbo].[NetPriceBuild]  WITH CHECK ADD  CONSTRAINT [FK_NetPriceBuild_NetPriceMain] FOREIGN KEY([MainId])
REFERENCES [dbo].[NetPriceMain] ([Id])
GO

ALTER TABLE [dbo].[NetPriceBuild] CHECK CONSTRAINT [FK_NetPriceBuild_NetPriceMain]
GO


