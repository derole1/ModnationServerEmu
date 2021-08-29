create table if not exists "policy" (
	"policy_id" integer not null primary key autoincrement,
	"policy_type" text check(policy_type IN ("EULA","PRIVACY")) not null,
	"platform" text check(platform IN ("PS2","PS3","PSP","WEB")) not null,
	"name" text,
	"content" text
);

create table if not exists "policy_accept" (
	"policy_accept_id" integer not null primary key autoincrement,
	"username" text not null unique,
	"policy_id" integer not null,
	"is_accepted" boolean not null default false,
	foreign key ("policy_id") references policy("policy_id"),
	unique ("username","policy_id")
);

create table if not exists "announcement" (
	"announcement_id" integer not null primary key autoincrement,
	"platform" text check(platform IN ("PS2","PS3","PSP","WEB")) not null,
	"subject" text,
	"language_code" text,
	"created_at" datetime not null default current_timestamp,
	"modified_at" datetime
);

create table if not exists "content_url" (
	"content_url_id" integer not null primary key autoincrement,
	"formats" text not null,
	"name" text not null
);

create table if not exists "content_update" (
	"content_update_id" integer not null primary key autoincrement,
	"platform" text check(platform IN ("PS2","PS3","PSP","WEB")) not null,
	"content_update_type" text check(content_update_type IN ("AUTO_CIRCLE_PLAYLIST","HOT_SEAT_PLAYLIST","ROM_STATUSES","THEMED_EVENTS")) not null,
	"content_url" text,
	"name" text,
	"uuid" text,
	"description" text,
	"has_been_uploaded" boolean,
	"created_at" datetime not null default current_timestamp,
	"updated_at" datetime
);

create table if not exists "server" (
	"server_id" integer not null primary key autoincrement,
	"address" text not null,
	"port" text not null,
	"server_type" text check(server_type IN ("BOMBD","DIRECTORY","FLS","JOB","NAT","RHTML","RXML","SIMSERVER")) not null,
	"server_version" text,
	"server_private_key" text
);

create table if not exists "player" (
	"player_id" integer not null primary key autoincrement,
	"username" text not null,
	"creator_points" integer,
	"creator_points_last_week" integer,
	"creator_points_this_week" integer,
	"experience_points" integer,
	"experience_points_last_week" integer,
	"experience_points_this_week" integer,
	"longest_drift" float,
	"longest_hang_time" float,
	"longest_win_streak" integer,
	"online_disconnected" integer,
	"online_finished" integer,
	"online_finished_this_week" integer,
	"online_finished_last_week" integer,
	"online_forfeit" integer,
	"online_races" integer,
	"online_races_this_week" integer,
	"online_races_last_week" integer
	"online_wins" integer,
	"online_wins_this_week" integer,
	"online_wins_last_week" integer,
	"win_streak" integer,
	"player_creation_quota" integer,
	"points" integer,
	"rank" integer,
	"rating" integer,
	"skill_level_id" integer,
	"star_rating" integer,
	"created_at" datetime default current_timestamp,
);

create table if not exists "player_profile" (
	"player_profile_id" integer not null primary key autoincrement,
	"player_id" integer not null unique,
	"first_name" text,
	"middle_name" text,
	"last_name" text,
	"addr_line1" text,
	"addr_line2" text,
	"addr_line3" text,
	"city" text,
	"state" text,
	"postal_code" text,
	"province" text,
	"country" text,
	"birthdate" datetime,
	"email" text,
	"email_confirmation" text,
	"cell_phone" text,
	"quote" text,
	"im_yahoo" text,
	"im_aol" text,
	"im_msn" text,
	"im_icq" text,
	"team_id" integer,
	foreign key ("player_id") references player("player_id")
	# TODO: team_id
);

create table if not exists "player_rating" (
	"player_rating_id" integer not null primary key autoincrement,
	"player_id" integer not null,
	"rating" integer not null,
	"comments" text,
	foreign key ("player_id") references player("player_id")
);

create table if not exists "psn_profile" (
	"psn_id" bigint not null primary key,
	"player_id" integer not null unique,
	"online_id" text,
	"console_id" text,
	"region" text,
	"domain" text,
	"service_id" text,
	"status" integer,
	"status_duration" integer,
	"date_of_birth" integer,
	foreign key ("player_id") references player("player_id")
);

create table if not exists "moderation_status" (
	"moderation_status_id" integer not null primary key autoincrement,
	"moderation_status" text
);

create table if not exists "player_creation" (
	"player_creation_id" integer not null primary key autoincrement,
	"player_id" integer,
	"name" text not null,
	"description" text,
	"tags" text,
	"player_creation_type" text check(player_creation_type IN ("CHARACTER","KART","PHOTO","TRACK")) not null,
	"moderation_status_id" integer,
	"parent_creation_id" integer,
	"parent_player_id" integer,
	"original_player_id" integer,
	"requires_dlc" boolean,
	"dlc_keys" text,
	"platform" text check(platform IN ("PS2","PS3","PSP","WEB")) not null,
	"is_remixable" boolean default true,
	"longest_hang_time" float,
	"longest_drift" float,
	"races_started" integer,
	"races_won" integer,
	"votes" integer,
	"races_finished" integer,
	"best_lap_time" float,
	"track_theme" integer,
	"auto_reset" boolean,
	"ai" boolean,
	foreign key ("player_id") references player("player_id"),
	foreign key ("moderation_status_id") references moderation_status("moderation_status_id"),
	foreign key ("parent_creation_id") references player_creation("player_creation_id"),
	foreign key ("parent_player_id") references player("player_id"),
	foreign key ("original_player_id") references player("player_id")
);

create table if not exists "player_creation_rating" (
	"player_creation_rating_id" integer not null primary key autoincrement,
	"player_creation_id" integer not null,
	"player_id" integer not null,
	"rating" integer not null,
	"comments" text,
	foreign key ("parent_creation_id") references player_creation("player_creation_id"),
	foreign key ("player_id") references player("player_id")
);

create table if not exists "game" (
	"game_id" integer not null primary key autoincrement,
	"game_type" text check(game_type IN ("CHARACTER_CREATORS","KART_CREATORS","ONLINE_ACTION_RACE","ONLINE_HOT_SEAT_RACE","ONLINE_LKS_RACE","ONLINE_PURE_RACE","ONLINE_TIME_TRIAL_RACE","OVERALL","OVERALL_CREATORS","OVERALL_RACE","TRACK_CREATORS")) not null,
	"game_state" text check(game_state IN ("ACTIVE","CANCELLED","CONCEDE","CONCEDE_ON","DISCONNECTED","DISCONNECTED_ON","DIVERGENCE","FINISHED","FORFEIT","FORFEIT_ON","FRIENDLY_QUIT","FRIENDLY_QUIT_ON","PENDING","PROCESSED","QUIT","QUIT_ON")) not null default "FINISHED",
	"host_player_id" integer,
	"platform" text check(platform IN ("PS2","PS3","PSP","WEB")) not null,
	"name" text,
	"is_ranked" boolean default true,
	"speed_class" text,
	"track" integer,
	"track_group" text,
	"privacy" text,
	"number_laps" integer,
	foreign key ("host_player_id") references player("player_id")
);

create table if not exists "game_player" (
	"game_player_id" integer not null primary key autoincrement,
	"player_id" integer,
	"team_id" integer,
	"game_state" text check(game_state IN ("ACTIVE","CANCELLED","CONCEDE","CONCEDE_ON","DISCONNECTED","DISCONNECTED_ON","DIVERGENCE","FINISHED","FORFEIT","FORFEIT_ON","FRIENDLY_QUIT","FRIENDLY_QUIT_ON","PENDING","PROCESSED","QUIT","QUIT_ON")) not null default "FINISHED",
	foreign key ("host_player_id") references player("player_id")
	# TODO: team_id
);

create table if not exists "game_player_stats" (
	"game_player_stats_id" integer not null primary key autoincrement,
	"is_complete" boolean not null default true,
	"track_idx" integer not null,
	"kart_idx" integer not null,
	"character_idx" integer not null,
	"best_lap_time" float not null,
	"music_idx" integer,
	"is_winner" integer,
	"finish_time" float,
	"bank" integer,
	"longest_drift" float,
	"longest_hang_time" float,
	"points" float,
	"volatility" float,
	"deviation" float,
	"finish_place" integer
);

create table if not exists "mail_message" (
	"mail_message_id" integer not null primary key autoincrement,
	"mail_message_type" text,
	"recipient_id" integer,
	"recipient_list" text,
	"sender_id" integer,
	"has_deleted" boolean,
	"has_forwarded" boolean,
	"has_read" boolean,
	"has_replied" boolean,
	"attachment_reference" text,
	"subject" text,
	"created_at" datetime not null default current_timestamp,
	"updated_at" datetime,
	foreign key ("recipient_id") references player("player_id"),
	foreign key ("sender_id") references player("player_id")
);

create table if not exists "achievement" (
	"achievement_id" integer not null primary key autoincrement,
	"achievement_type_id" integer not null,
	"has_read" boolean not null default false,
	"player_id" integer,
	"player_creation_id" integer,
	"relevant" boolean,
	"value" integer,
	"created_at" datetime not null default current_timestamp,
	"updated_at" datetime,
	foreign key ("player_id") references player("player_id"),
	foreign key ("parent_creation_id") references player_creation("player_creation_id")
);

create table if not exists "player_complaint" (
	"player_complaint_id" integer not null primary key autoincrement,
	"player_id" integer not null,
	"player_complaint_reason" text check(player_complaint_reason IN ("COPYRIGHT","HARASS","ILLEGAL","MATURE","OFFENSIVE","OTHER","RACIAL","SEXUAL","TOS","VIOLENCE","VULGAR")) not null,
	"player_comments" text,
	foreign key ("player_id") references player("player_id")
);