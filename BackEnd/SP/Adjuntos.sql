/****** Object:  Table [dbo].[Adjuntos]    Script Date: 08/08/2014 04:27:29 p.m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Adjuntos' and xtype='U')
BEGIN
	CREATE TABLE [dbo].[Adjuntos](
		[IdAdjunto] [int] IDENTITY(1,1) NOT NULL,
		[IdGrupo] [varchar](36) NULL,
		[Descripcion] [varchar](50) NULL,
		[Archivo] [varbinary](max) NULL,
		[NombreArchivo] [varchar](120) NULL,
		[Tipo] [varchar](50) NULL,
		[IdUsuario] [varchar](25) NULL,
		[IdPerfil] [varchar](250) NULL,
		[IdRelacion] [int] NULL,
	 CONSTRAINT [PK_Adjuntos] PRIMARY KEY CLUSTERED 
	(
		[IdAdjunto] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

SET ANSI_PADDING OFF
GO

