CREATE DATABASE CONTROLEDETAREFAS;

USE CONTROLEDETAREFAS;
USE CONTROLEDEPONTO;
drop database CONTROLEDETAREFAS
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

INSERT INTO Funcionarios(NomeDoFuncionario, DataDeAdmissao, NascimentoDoFuncionario, Cpf,CelularDoFuncionario, EmailDoFuncionario, SenhaDoFuncionario, Perfil)
VALUES('Administrador', '2000-01-01', '2000-01-01','66136918013', '999999999','administrador@administrador.com', '3627909a29c31381a071ec27f7c9ca97726182aed29a7ddd2e54353322cfb30abb9e3a6df2ac2c20fe23436311d678564d0c8d305930575f60e2d3d048184d79', 1 );        

drop database Funcionarios

CREATE TABLE EmpresasCliente(
	EmpresaClienteId INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	RazaoSocial VARCHAR(255) NOT NULL,
	Cnpj VARCHAR(255) NOT NULL,
	EnderecoDaEmpresa VARCHAR(255) NOT NULL,
	DataDeInclusaoDaEmpresa datetime NOT NULL,
	NomeGestorDoContrato VARCHAR(255) NOT NULL,
	EmailGestorDoContrato VARCHAR(255) NOT NULL
);

DROP TABLE EmpresasCliente

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






DROP TABLE Tarefas
insert into Cargos values('Administrador');
insert into Cargos values('Recursos Humanos');
insert into Cargos values('Diretoria');
insert into Cargos values('Colaborador');

INSERT INTO Funcionarios(NomeDoFuncionario, Cpf, NascimentoFuncionario, DataDeAdmissao, CelularFuncionario,EmailFuncionario, SenhaFuncionario, CargoId)
VALUES('Administrador', '66136918013', '2000-01-01', '2000-01-01', '999999999','administrador@administrador.com', '12345', 1 );

insert into EmpresasCliente values('FIEMG','123456789123', 'Avenida A','2022-01-02', 'Nayara', 'nayara@nayara.com' )

insert into Tarefas values(1,1, 'Teste unitatio', 'teste Unitário completo da solucção', 1,'2022-01-01','2022-01-02', null )

select * from Funcionarios

select * from EmpresasCliente

select * from Tarefas

delete from Funcionarios where FuncionarioId = 1

update Tarefas set DataHorarioFimTarefa = null
select * from Tarefas


SELECT 
                                         t.FuncionarioId

                                         FROM Tarefas t INNER JOIN Funcionarios f ON f.FuncionarioId = t.FuncionarioId
                                             INNER  JOIN ClientesEmpresa c ON c.EmpresaClienteId = t.EmpresaClienteId;

SELECT 
                                         t.FuncionarioId
											 from bens_patrimoniais a
inner join tipo_bens b on a.tipo_bens = b.cod_bensT
inner join categoria_bens c on a.cat_bens = c.cod_bens;