BEGIN TRANSACTION;
GO

ALTER TABLE [Ventas] ADD [MontoDelivery] decimal(18,2) NOT NULL DEFAULT 0.0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260409203341_AgregarMontoDeliveryEnVenta', N'8.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260409203616_AjustarPrecisionMontoDelivery', N'8.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [PagosDelivery] (
    [Id] int NOT NULL IDENTITY,
    [DeliveryId] int NOT NULL,
    [Fecha] datetime2 NOT NULL,
    [Monto] decimal(18,2) NOT NULL,
    [FormaPago] int NOT NULL,
    [Observacion] nvarchar(max) NULL,
    CONSTRAINT [PK_PagosDelivery] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PagosDelivery_Deliveries_DeliveryId] FOREIGN KEY ([DeliveryId]) REFERENCES [Deliveries] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [PagosDeliveryDetalle] (
    [Id] int NOT NULL IDENTITY,
    [PagoDeliveryId] int NOT NULL,
    [PedidoId] int NOT NULL,
    [MontoEnvio] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_PagosDeliveryDetalle] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PagosDeliveryDetalle_PagosDelivery_PagoDeliveryId] FOREIGN KEY ([PagoDeliveryId]) REFERENCES [PagosDelivery] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PagosDeliveryDetalle_Pedidos_PedidoId] FOREIGN KEY ([PedidoId]) REFERENCES [Pedidos] ([Id]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX_PagosDelivery_DeliveryId] ON [PagosDelivery] ([DeliveryId]);
GO

CREATE INDEX [IX_PagosDeliveryDetalle_PagoDeliveryId] ON [PagosDeliveryDetalle] ([PagoDeliveryId]);
GO

CREATE INDEX [IX_PagosDeliveryDetalle_PedidoId] ON [PagosDeliveryDetalle] ([PedidoId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260410042907_CrearPagoDelivery', N'8.0.10');
GO

COMMIT;
GO

