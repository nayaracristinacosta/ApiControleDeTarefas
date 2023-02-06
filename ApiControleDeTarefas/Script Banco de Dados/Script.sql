CREATE DATABASE CONTROLEDETAREFAS;

USE CONTROLEDETAREFAS;

CREATE TABLE Funcionarios(
	FuncionarioId INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	NomeDoFuncionario VARCHAR(255) NOT NULL,
	DataDeAdmissao DATE NOT NULL,
    NascimentoDoFuncionario DATE NOT NULL,
	Cpf VARCHAR(14) NOT NULL,	
	CelularDoFuncionario VARCHAR(11) NOT NULL,
	EmailDoFuncionario VARCHAR(255) NOT NULL,
	SenhaDoFuncionario VARCHAR(255) NOT NULL,
	Perfil int NOT NULL,
	TokenEmail int null
);

CREATE TABLE EmpresasCliente(
	EmpresaClienteId INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	RazaoSocial VARCHAR(255) NOT NULL,
	Cnpj VARCHAR(255) NOT NULL,
	EnderecoDaEmpresa VARCHAR(255) NOT NULL,
	DataDeInclusaoDaEmpresa datetime NOT NULL,
	NomeGestorDoContrato VARCHAR(255) NOT NULL,
	EmailGestorDoContrato VARCHAR(255) NOT NULL
);


CREATE TABLE Tarefas(
	TarefaId INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	FuncionarioId INT NOT NULL REFERENCES Funcionarios(FuncionarioId), 
	EmpresaClienteId INT NOT NULL REFERENCES EmpresasCliente(EmpresaClienteId),
	AssuntoTarefa VARCHAR(50) NOT NULL,
	Descricao VARCHAR(255) NOT NULL,
	TipoDaTarefa INT NOT NULL,
	DataHorarioInicioTarefa DATETIME NOT NULL,
	DataHorarioFimTarefa DATETIME NULL,
	TempoTotalGastoTarefa VARCHAR(30) NULL,

);



INSERT INTO Funcionarios(NomeDoFuncionario, DataDeAdmissao, NascimentoDoFuncionario, Cpf,CelularDoFuncionario, EmailDoFuncionario, SenhaDoFuncionario, Perfil)
VALUES('Administrador', '2000-01-01', '2000-01-01','66136918013', '999999999','administrador@administrador.com', '3627909a29c31381a071ec27f7c9ca97726182aed29a7ddd2e54353322cfb30abb9e3a6df2ac2c20fe23436311d678564d0c8d305930575f60e2d3d048184d79', 1 );        



