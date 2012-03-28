   -- Creted with SQLite Expert Personal 3.4.2

CREATE TABLE [accounts] (
  [id] INTEGER  NOT NULL,
  [email] TEXT  NOT NULL,
  [salt] VARBINARY(32)  NOT NULL,
  [passwordVerifier] VARBINARY(128)  NOT NULL,
  [battletagname] TEXT  NOT NULL,
  [hashCode] INTEGER  NOT NULL,
  [userLevel] INTEGER DEFAULT '0' NOT NULL,
  [LastSelectedHeroId] INTEGER, 
  [LastOnline] INTEGER DEFAULT 0);


CREATE TABLE [active_skills] (
  [id_toon] INTEGER  NULL,
  [skill_0] INTEGER  NULL,
  [rune_0] INTEGER  NULL,
  [skill_1] INTEGER  NULL,
  [rune_1] INTEGER  NULL,
  [skill_2] INTEGER  NULL,
  [rune_2] INTEGER  NULL,
  [skill_3] INTEGER  NULL,
  [rune_3] INTEGER  NULL,
  [skill_4] INTEGER  NULL,
  [rune_4] INTEGER  NULL,
  [skill_5] INTEGER  NULL,
  [rune_5] INTEGER  NULL,
  [passive_0] INTEGER  NULL,
  [passive_1] INTEGER  NULL,
  [passive_2] INTEGER  NULL);


CREATE TABLE [friendInvites] (
  [inviterId] BIGINT(255) NOT NULL, 
  [inviteeId] BIGINT(255) NOT NULL, 
  [message] VARCHAR(255));


CREATE TABLE [friends] (
  [accountId] BIGINT(255) NOT NULL, 
  [friendId] BIGINT(255) NOT NULL);


CREATE TABLE [gameaccounts] (
  [id] BIGINT(255) NOT NULL, 
  [accountid] BIGINT(255) NOT NULL, 
  [banner] BLOB);


CREATE TABLE [inventory] (
  [account_id] INTEGER, 
  [toon_id] INTEGER, 
  [inventory_loc_x] INTEGER,
  [inventory_loc_y] INTEGER,
  [equipment_slot] INTEGER,
  [item_id] INTEGER);


CREATE TABLE [item_entities] (
  [id] INTEGER PRIMARY KEY,
  [item_gbid] INTEGER,
  [item_attributes] TEXT,
  [item_affixes] TEXT);


CREATE TABLE [toons] (
  [id] INTEGER, 
  [name] VARCHAR(255), 
  [class] INTEGER, 
  [gender] INTEGER, 
  [level] INTEGER, 
  [experience] INTEGER, 
  [accountId] INTEGER, 
  [hashCode] INTEGER, 
  [timePlayed] INTEGER);
