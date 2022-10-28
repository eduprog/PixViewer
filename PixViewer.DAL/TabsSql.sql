-- creaete order: 1
create table tb_profile (
id int identity,
profile_description varchar(60) not null,
create_date datetime not null,
active bit not null
primary key(id)
)

-- create order: 2
create table tb_actions (
id int identity,
action_description varchar(60) not null,
create_date datetime not null,
active bit not null,
primary key(id)
)

-- create order: 3
create table tb_user_actions (
profile_id int not null,
action_id int not null,
constraint fk_tb_user_actions_tb_profile foreign key (profile_id) references tb_profile on delete cascade on update cascade,
constraint fk_tb_user_actions_tb_action foreign key (action_id) references tb_actions on delete cascade on update cascade
)

-- create order: 4
create table tb_user (
id int identity,
profile_id int not null,
full_name varchar(80) not null,
cre_login varchar(20) unique not null,
cre_password varchar(100) not null,
create_date datetime not null,
last_access_date datetime null,
max_webhooks_availables int,
active bit not null,
primary key(id),
constraint fk_tb_user_tb_profile foreign key (profile_id) references tb_profile on delete cascade on update cascade
)

-- create order: 5
create table tb_webhook (
id int identity,
client_id int not null,
register_date datetime not null,
pix_key varchar(20) unique not null,
last_update datetime not null,
active bit not null,
primary key(id),
constraint fk_tb_webhook_tb_user_client foreign key (client_id) references tb_user on delete cascade on update cascade
)

-- create order: 6
create table tb_historic_user (
id int identity,
profile_id int not null,
full_name varchar(80) not null,
cre_login varchar(20) not null,
cre_password varchar(100) not null,
create_date datetime not null,
historic_date datetime not null,
last_access_date datetime null,
max_webhooks_availables int,
action_executed int not null,
active bit not null,
primary key(id)
)

-- create order: 7
create table tb_historic_webhook (
id int identity,
client_id int not null,
register_date datetime not null,
pix_key varchar(20) not null,
historic_date datetime not null,
last_update datetime not null,
action_executed int not null,
active bit not null,
primary key(id)
)

-- create order: 8
create table tb_historic_cob (
id int identity,
requester_id int not null,
qrcode varchar(150) not null,
copy_paste varchar(max) not null,
cob_status varchar(35) not null,
expire_time int not null,
cob_location varchar(100) not null,
tx_id varchar(50) not null,
debtor_name varchar(75) not null,
debtor_cpf varchar(12) not null,
pix_key varchar(70) not null,
payer_description varchar(255) not null,
additional_infos varchar(255) not null,
historic_date datetime not null,
primary key(id)
)


-- ===============> DROP TABLE ORDER <================ 

 --DROP TABLE TB_HISTORIC_USER				-- ORDER 1
 --DROP TABLE TB_HISTORIC_WEBHOOK			-- ORDER 2
 --DROP TABLE TB_WEBHOOK					-- ORDER 3
 --DROP TABLE TB_USER						-- ORDER 4
 --DROP TABLE TB_USER_ACTIONS				-- ORDER 5
 --DROP TABLE TB_ACTIONS					-- ORDER 6
 --DROP TABLE TB_PROFILE					-- ORDER 7
 --DROP TABLE TB_HISTORIC_COB				-- ORDER 8
GO
