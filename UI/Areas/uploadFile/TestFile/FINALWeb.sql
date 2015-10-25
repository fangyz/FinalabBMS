--创建FINALWeb数据库
create database FINALWeb

--切换到FINALWeb数据库
use FINALWeb

--创建消息表Tbl_Message

--编号	id	int	是	否
--标题	title	nvarcher（50）	否	否
--内容	content	ntext 	否	否
--发送时间	sendTime	datetime	否	是
--附件	Atachment vachar

create table Tbl_Message(
	Id int constraint PK_Tbl_Message_Id primary key not null ,
	Title nvarchar(50) not null ,
	Content ntext not null ,
	Atachment varchar(50),
	SendTime datetime 
)

--创建消息联系表Tbl_User_Message
--名称	别名	数据类型	主键	是否为空
--编号	id	int	是	否
--收件人编号	receiveId	int	否	否
--发送人编号	sendId	int 	否	否
--消息编号	messageId	int	否	否
--阅读	isRead	bit	否	否
--收件人删除	receiveIsDelete	bit	否	否
--草稿	isDraft	bit	否	否
--发送人删除	sendIsDelete	bit	否	否

create table Tbl_User_Message(
	Id int constraint PK_Tbl_User_Message_Id primary key not null ,
	ReceiveId int not null ,
	SendId int not null ,
	MessageId int constraint FK_Tbl_User_Message_MessageId foreign key  references  Tbl_Message(Id) not null ,
	IsRead bit constraint DF_Tbl_User_Message_IsRead default(0) not null ,
	ReceiveIsDelete bit constraint DF_Tbl_User_Message_ReceiveIsDelete default(0) not null ,
	IsDraft bit constraint DF_Tbl_User_Message_IsDraft default(0) not null ,
	SendIsDelete bit constraint DF_Tbl_User_Message_SendIsDelete default(0) not null
)

--创建系统消息表
--编号	Id	int
--发送Id	SendId	int
--标题	Title	nvarchar(50)
--内容	Content	ntext 
--附件	Atachment	varchar(100)
--时间	SendTime	datetime

create table SystemMessage(
	Id int  constraint PK_SystemMessage_Id primary key identity(1,1) not null ,
	SendId int not null ,
	Title nvarchar(100) not null ,
	Content ntext not null ,
	Atachment varchar(100) null ,
	SendTime datetime not null
)


--创建发布消息的存储过程插入消息 usp_sendMessage

create proc usp_insertMessage

@Title nvarchar(50),
@Content ntext,
@Atachment varchar(50),
@SendTime datetime , 

@MesageId int out
as
begin
	insert into Tbl_Message(Title , Content ,  Atachment ,SendTime ) 
	values(@Title , @Content , @Atachment , @SendTime)
	
	set @MesageId=@@IDENTITY
end

--测试存储过程
declare @st int 
exec usp_insertMessage @Title = '作业布置2' , @Content = '完成三层' , @Atachment = 'D:/text' , @SendTime = '2008-05-10' ,@MesageId = @st  output
print @st


--查询所有消息数据
select * from Tbl_Message


--创建存储过程发布消息 usp_sendMessage

create proc usp_sendMessage
@ReceiveId int ,
@SendId int ,
@MessageId int 
as
begin
	insert into Tbl_User_Message(ReceiveId , SendId , MessageId ) 
	values(@ReceiveId , @SendId , @MessageId )
end

--测试存储过程
exec usp_sendMessage @ReceiveId = 1 , @SendId = 1 , @MessageId = 11 --成功执行
exec usp_sendMessage @ReceiveId = 1 , @SendId = 1 , @MessageId = 20 --外键约束不能插入


--创建存储过程查看信息 usp_ViewMessage
create proc usp_ViewMessage
@ReceiveId int ,
@MessageCount int out
as 
begin
	select SendId , MessageId , IsRead  
	from Tbl_User_Message 
	where ReceiveId = @ReceiveId and ReceiveIsDelete = 0
	order by Id desc
	
	select @MessageCount = count(*) 
	from Tbl_User_Message 
	where ReceiveId = @ReceiveId and ReceiveIsDelete = 0 and IsRead = 0
end

--测试存储过程
declare @MC int 
exec usp_ViewMessage @ReceiveId = 1 , @MessageCount = @MC output
print @MC

select * from Tbl_User_Message