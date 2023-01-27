-- TABLE
CREATE TABLE `AccionBitacora` (
	`ID` VARCHAR(1) NOT NULL,
	`Accion` VARCHAR(15) NOT NULL,
	`creado` DATETIME NOT NULL,
	`actualizado` DATETIME NOT NULL,
	PRIMARY KEY (`ID`)
);
CREATE TABLE `Bitacora` (
	`ID` BIGINT NOT NULL,
	`Tabla` VARCHAR(20) NOT NULL,
	`Registro` TEXT NOT NULL,
	`AccionID` VARCHAR(1) NOT NULL,
	`fechaRegisro` DATETIME NOT NULL,
	`creado` DATETIME NOT NULL,
	`actualizado` DATETIME NOT NULL,
	PRIMARY KEY (`ID`)
);
CREATE TABLE `Detalle` (
	`ID` BIGINT NOT NULL,
	`UUID` VARCHAR(200) NOT NULL,
	`Concepto` TEXT NOT NULL,
	`PrecioUnitario` REAL NOT NULL,
	`Cantidad` REAL NOT NULL,
	`Total` REAL NOT NULL,
	PRIMARY KEY (`ID`, `UUID`)
);
CREATE TABLE `Factura` (
	`UUID` VARCHAR(200) NOT NULL,
	`FOLIO` VARCHAR(200) NOT NULL,
	`SERIE` VARCHAR(10) NOT NULL,
	`Emisor` VARCHAR(20) NOT NULL,
	`Receptor` VARCHAR(20) NOT NULL,
	"FechaTimbrado" DATETIME NOT NULL,
	`statusID` VARCHAR(5) NOT NULL,
	`creado` DATETIME NOT NULL,
	`actualizado` DATETIME NOT NULL, archivo BLOB,
	PRIMARY KEY (`UUID`, `FOLIO`, `SERIE`),
	FOREIGN KEY (`statusID`) REFERENCES `StatusFactura`(`ID`),
	FOREIGN KEY (`Emisor`) REFERENCES `Persona`(`ID`),
	FOREIGN KEY (`Receptor`) REFERENCES `Persona`(`ID`)
);
CREATE TABLE `Persona` (
	`ID` VARCHAR(20) NOT NULL,
	`Tipo` VARCHAR(1) NOT NULL,
	`Descripcion` TEXT NOT NULL,
	`statusID` VARCHAR(1) NOT NULL,
	`creado` DATETIME NOT NULL,
	`actualizado` DATETIME NOT NULL,
	PRIMARY KEY (`ID`, `Tipo`),
	UNIQUE (`ID`),
	FOREIGN KEY (`Tipo`) REFERENCES `TipoPersona`(`ID`),
	FOREIGN KEY (`statusID`) REFERENCES `Status`(`ID`)
);
CREATE TABLE `PersonaRelacion` (
	`PersonaID` VARCHAR(20) NOT NULL,
	`TipoPersonaID` VARCHAR(1) NOT NULL,
	`TipoRelacionID` VARCHAR(1) NOT NULL,
	`statusID` VARCHAR(1) NOT NULL,
	`creado` DATETIME NOT NULL,
	`actualizado` DATETIME NOT NULL,
	PRIMARY KEY (`PersonaID`, `TipoPersonaID`, `TipoRelacionID`),
	FOREIGN KEY (`TipoRelacionID`) REFERENCES `TipoRelacion`(`ID`),
	FOREIGN KEY (`PersonaID`, `TipoPersonaID`) REFERENCES `Persona`(`ID`,`Tipo`),
	FOREIGN KEY (`statusID`) REFERENCES `Status`(`ID`)
);
CREATE TABLE `Status` (
	`ID` VARCHAR(1) NOT NULL,
	`status` VARCHAR(10) NOT NULL,
	`creado` DATETIME NOT NULL,
	`actualizado` DATETIME NOT NULL,
	PRIMARY KEY (`ID`)
);
CREATE TABLE `StatusFactura` (
	`ID` VARCHAR(5) NOT NULL,
	`status` VARCHAR(50) NOT NULL,
	`creado` DATETIME NOT NULL,
	`actualizado` DATETIME NOT NULL,
	PRIMARY KEY (`ID`)
);
CREATE TABLE `TipoPersona` (
	`ID` VARCHAR(1) NOT NULL,
	`Tipo` VARCHAR(6) NOT NULL,
	`statusID` VARCHAR(1) NOT NULL,
	`creado` DATETIME NOT NULL,
	`actualizado` DATETIME NOT NULL,
	PRIMARY KEY (`ID`),
	FOREIGN KEY (`statusID`) REFERENCES `Status`(`ID`)
);
CREATE TABLE `TipoRelacion` (
	`ID` VARCHAR(1) NOT NULL,
	`Tipo` VARCHAR(10) NOT NULL,
	`statusID` VARCHAR(1) NOT NULL,
	`creado` DATETIME NOT NULL,
	`actualizado` DATETIME NOT NULL,
	PRIMARY KEY (`ID`),
	FOREIGN KEY (`statusID`) REFERENCES `Status`(`ID`)
);
CREATE TABLE XmlNs(
    ID INTEGER PRIMARY KEY,
    ns VARCHAR(10),
    valor TEXT,
    statusID VARCHAR(1),
    creado TIMESTAMP,
    actualizado TIMESTAMP,
    Foreign KEY (`StatusID`) References `Status` (`ID`)
);
 
-- INDEX
 
-- TRIGGER
 
-- VIEW
 
