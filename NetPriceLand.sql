/****** Object:  Table [dbo].[NetPriceLand]    Script Date: 2026/1/5 ¤W¤È 10:29:58 ******/
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

ALTER TABLE [dbo].[NetPriceLand]  WITH CHECK ADD  CONSTRAINT [FK_NetPriceLand_NetPriceMain] FOREIGN KEY([MainId])
REFERENCES [dbo].[NetPriceMain] ([Id])
GO

ALTER TABLE [dbo].[NetPriceLand] CHECK CONSTRAINT [FK_NetPriceLand_NetPriceMain]
GO