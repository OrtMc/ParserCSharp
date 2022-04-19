CREATE TABLE [dbo].[Offers] (
    [Id]                    INT            IDENTITY (1, 1) NOT NULL,
    [type]                  NVARCHAR (20)  NULL,
    [bid]                   INT            NULL,
    [cbid]                  INT            NULL,
    [available]             VARCHAR (5)    NULL,
    [offer_id]              INT            NULL,
    [url]                   NVARCHAR (80)  NULL,
    [price]                 INT            NULL,
    [currencyid]            NVARCHAR (3)   NULL,
    [categoryid]            INT            NULL,
    [picture]               NVARCHAR (50)  NULL,
    [delivery]              NVARCHAR (5)   NULL,
    [local_delivery_cost]   INT            NULL,
    [typeprefix]            NVARCHAR (20)  NULL,
    [vendor]                NVARCHAR (10)  NULL,
    [vendorcode]            NVARCHAR (10)  NULL,
    [model]                 NVARCHAR (20)  NULL,
    [description]           NVARCHAR (MAX) NULL,
    [manufacturer_warranty] NVARCHAR (5)   NULL,
    [country_of_origin]     NVARCHAR (20)  NULL,
    [author]                NVARCHAR (20)  NULL,
    [name]                  NVARCHAR (100) NULL,
    [publisher]             NVARCHAR (30)  NULL,
    [series]                NVARCHAR (80)  NULL,
    [year]                  SMALLINT       NULL,
    [isbn]                  NVARCHAR (20)  NULL,
    [volume]                INT            NULL,
    [part]                  INT            NULL,
    [language]              NVARCHAR (5)   NULL,
    [binding]               NVARCHAR (15)  NULL,
    [page_extent]           INT            NULL,
    [downloadable]          NVARCHAR (5)   NULL,
    [performed_by]          NVARCHAR (20)  NULL,
    [performance_type]      NVARCHAR (20)  NULL,
    [storage]               NVARCHAR (20)  NULL,
    [format]                NVARCHAR (20)  NULL,
    [recording_length]      NVARCHAR (10)  NULL,
    [artist]                NVARCHAR (30)  NULL,
    [title]                 NVARCHAR (50)  NULL,
    [media]                 NVARCHAR (10)  NULL,
    [starring]              NVARCHAR (100) NULL,
    [director]              NVARCHAR (30)  NULL,
    [originalname]          NVARCHAR (30)  NULL,
    [country]               NVARCHAR (20)  NULL,
    [worldregion]           NVARCHAR (15)  NULL,
    [region]                NVARCHAR (15)  NULL,
    [days]                  TINYINT        NULL,
    [datatour]              NVARCHAR (50)  NULL,
    [hotel_stars]           NVARCHAR (10)  NULL,
    [room]                  NVARCHAR (10)  NULL,
    [meal]                  NVARCHAR (10)  NULL,
    [included]              NVARCHAR (100) NULL,
    [transport]             NVARCHAR (20)  NULL,
    [place]                 NVARCHAR (50)  NULL,
    [hall]                  NVARCHAR (20)  NULL,
    [hall_part]             NVARCHAR (20)  NULL,
    [date]                  NVARCHAR (50)  NULL,
    [is_premiere]           CHAR (1)       NULL,
    [is_kids]               CHAR (1)       NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

