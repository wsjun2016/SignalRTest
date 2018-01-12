use Live_Test
go

if exists(select 1 from sysobjects where name='UserInfo' and type='U')
drop table UserInfo
go
--用户表
create table UserInfo(
UserID int primary key identity(1,1),
UserName nvarchar(50) not null,
HeadPic nvarchar(500) null,
LoginID varchar(20) not null
)
go

if exists(select 1 from sysobjects where name='LiveData' and type='U')
drop table LiveData
go
--直播列表
create table LiveData(
ID int primary key identity(1,1),
Title nvarchar(100) null,
FrontCover varchar(500) null,
ViewCount int not null default 0
)
go

if exists(select 1 from sysobjects where name='LiveChatRoom' and type='U')
drop table LiveChatRoom
go
create table LiveChatRoom(
ID bigint primary key identity(1,1),
LiveID int not null,--直播ID
Status int not null,--1:开放 0:关闭
)
go

if exists(select 1 from sysobjects where name='LiveChatRoomMember' and type='U')
drop table LiveChatRoomMember
go
create table LiveChatRoomMember(
ID bigint primary key identity(1,1),
RoomID bigint not null,--直播聊天室房间ID
ConnectionID varchar(50) null,--signalr 连接的connectionID
UserID int null--用户ID
)
go

--新增测试数据
if not exists(select 1 from LiveData with(nolock) where Title='非洲大野象')
begin
	insert into LiveData(Title,FrontCover,ViewCount) values('非洲大野象','https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1515752734437&di=e8bf5c98084a9c842ff66f95af5d1c51&imgtype=0&src=http%3A%2F%2Fimg3.3lian.com%2F2006%2F004%2F19%2F071.jpg',387523)
end
go
if not exists(select 1 from LiveData with(nolock) where Title='我爱大自然')
begin
	insert into LiveData(Title,FrontCover,ViewCount) values('我爱大自然','https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1515752870338&di=b8ccfc618b489043692607e64440c648&imgtype=0&src=http%3A%2F%2Fbpic.ooopic.com%2F15%2F06%2F82%2F15068218-0ec92511e566b59926b5f142dc0c0db6.jpg',465234)
end
go
if not exists(select 1 from LiveData with(nolock) where Title='九星连珠')
begin
	insert into LiveData(Title,FrontCover,ViewCount) values('九星连珠','https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1515752942811&di=a33220fd321e9ccc811fb9b8fa331bba&imgtype=0&src=http%3A%2F%2Fpic.58pic.com%2F58pic%2F15%2F61%2F93%2F36d58PICxwa_1024.jpg',52343)
end
go
if not exists(select 1 from LiveData with(nolock) where Title='日食')
begin
	insert into LiveData(Title,FrontCover,ViewCount) values('日食','https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1515752992156&di=731ec490f8d2d21f70d1f07e33881c63&imgtype=0&src=http%3A%2F%2Fimg1.sc115.com%2Fuploads%2Fsc%2Fjpg%2F141%2F9265.jpg',233562)
end
go
if not exists(select 1 from LiveData with(nolock) where Title='自由探险')
begin
	insert into LiveData(Title,FrontCover,ViewCount) values('自由探险','https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1515753067119&di=68cf711841cec830a6507bb7d2014a53&imgtype=0&src=http%3A%2F%2Fupload.news.cecb2b.com%2F2014%2F1127%2F1417057597604.jpg',235623)
end
go

if not exists(select 1 from UserInfo with(nolock) where UserName='青云飘零')
begin
	insert into UserInfo(UserName,HeadPic,LoginID) values('青云飘零','https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1515647390623&di=1e15100d2044e87f5de5d30113fc1e80&imgtype=jpg&src=http%3A%2F%2Fimg1.imgtn.bdimg.com%2Fit%2Fu%3D3108820096%2C3322755446%26fm%3D214%26gp%3D0.jpg',1)
end
go
if not exists(select 1 from UserInfo with(nolock) where UserName='青龙')
begin
	insert into UserInfo(UserName,HeadPic,LoginID) values('青龙','https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1515652019378&di=d6fc0ec7890d107a26cab120278ca2d0&imgtype=0&src=http%3A%2F%2Fa3.att.hudong.com%2F27%2F62%2F01300542493094139873620335330.jpg',2)
end
go
if not exists(select 1 from UserInfo with(nolock) where UserName='白虎')
begin
	insert into UserInfo(UserName,HeadPic,LoginID) values('白虎','https://ss0.bdstatic.com/70cFuHSh_Q1YnxGkpoWK1HF6hhy/it/u=1801044725,3953009433&fm=27&gp=0.jpg',3)
end
go
if not exists(select 1 from UserInfo with(nolock) where UserName='朱雀')
begin
	insert into UserInfo(UserName,HeadPic,LoginID) values('朱雀','https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1515652101454&di=58439b133a1fed60dae22d114d0f51bb&imgtype=0&src=http%3A%2F%2Fs16.sinaimg.cn%2Fmw690%2F91bc36e9tx6DjQOFeV19f%26690',4)
end
go
if not exists(select 1 from UserInfo with(nolock) where UserName='玄武')
begin
	insert into UserInfo(UserName,HeadPic,LoginID) values('玄武','https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1515652145439&di=da1bc13c32e43c04ab8214cf82afdb49&imgtype=0&src=http%3A%2F%2Fd.hiphotos.baidu.com%2Fzhidao%2Fpic%2Fitem%2Fac4bd11373f08202b975aef449fbfbedaa641b11.jpg',5)
end
go