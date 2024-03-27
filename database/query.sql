USE master;
GO

-- delete facebook_db database if exists
DECLARE @databaseName VARCHAR(50) = (SELECT name FROM sys.databases WHERE name = 'facebook_db')

IF (@databaseName IS NOT NULL)
BEGIN
	-- Drop all connections to the target database
	DECLARE @SQL varchar(max)

	SELECT @SQL = COALESCE(@SQL,'') + 'Kill ' + Convert(varchar, SPId) + ';'
	FROM 
		MASTER..SysProcesses
	WHERE 
		DBId = DB_ID(@databaseName) AND 
		SPId <> @@SPId

	EXEC(@SQL)

	DROP DATABASE facebook_db;
END
GO

CREATE DATABASE facebook_db
GO

USE facebook_db
GO

-- TABLES
CREATE TABLE tb_user
(
	id INT IDENTITY(1,1) PRIMARY KEY,
	first_name VARCHAR(250) NOT NULL,
	last_name VARCHAR(250) NOT NULL,
	email VARCHAR(250) NOT NULL,
	bio TEXT,
	image_name VARCHAR(250),
	date_of_birth DATE,
	hash VARCHAR(250) NOT NULL,
	email_token VARCHAR(250),
	email_token_expires_at DATETIME,
	email_verified_at DATETIME,
	password_token VARCHAR(250),
	password_token_expires_at DATETIME,
	created_at DATETIME DEFAULT GETDATE()
)
GO

CREATE TABLE tb_post
(
	id INT IDENTITY(1,1) PRIMARY KEY,
	user_id INT NOT NULL,
	message VARCHAR(500),
	media_type VARCHAR(250),
	file_name VARCHAR(250),
	is_active BIT DEFAULT 1,
	updated_at DATETIME DEFAULT NULL,
	created_at DATETIME DEFAULT GETDATE()

	FOREIGN KEY(user_id) REFERENCES tb_user(id)
)
GO

CREATE TABLE tb_friendship
(
	source_id INT,
	target_id INT,
	status VARCHAR(250) DEFAULT 'pending',
	updated_at DATETIME DEFAULT NULL,
	created_at DATETIME DEFAULT GETDATE()

	PRIMARY KEY(source_id, target_id),
	FOREIGN KEY(source_id) REFERENCES tb_user(id),
	FOREIGN KEY(target_id) REFERENCES tb_user(id)
)
GO

CREATE TABLE tb_comment
(
	id INT IDENTITY(1,1) PRIMARY KEY,
	post_id INT,
	user_id INT,
	comment VARCHAR(500),
	media_type VARCHAR(250),
	file_name VARCHAR(250),
	is_active BIT DEFAULT 1,
	updated_at DATETIME DEFAULT NULL,
	created_at DATETIME DEFAULT GETDATE()

	FOREIGN KEY(user_id) REFERENCES tb_user(id),
	FOREIGN KEY(post_id) REFERENCES tb_post(id),
)
GO

CREATE TABLE tb_reaction
(
	id INT IDENTITY(1,1) PRIMARY KEY,
	user_id INT,
	entity_id INT,							-- comment_id | post_id
	type VARCHAR(250),						-- comment | post
	created_at DATETIME DEFAULT GETDATE()

	FOREIGN KEY (user_id) REFERENCES tb_user(id)
)
GO

-- USERS
CREATE PROCEDURE pr_user_create
	@first_name VARCHAR(250),
	@last_name VARCHAR(250),
	@email VARCHAR(250),
	@hash VARCHAR(250),
	@email_token VARCHAR(250),
	@date_of_birth DATE
AS
	INSERT INTO tb_user (first_name, last_name, email, hash, email_token, email_token_expires_at, date_of_birth)
	VALUES
		(@first_name, @last_name, @email, @hash, @email_token, DATEADD(DAY, 1, GETDATE()), @date_of_birth)
GO

CREATE PROCEDURE pr_user_edit
	@user_id INT,
	@bio TEXT,
	@image_name VARCHAR(250) = NULL
AS
	UPDATE tb_user
	SET
		bio = @bio,
		image_name = @image_name
	WHERE 
		id = @user_id
GO

CREATE PROCEDURE pr_user_get_by_token
	@token VARCHAR(250),
	@type VARCHAR(250)
AS
	SELECT 
		id						=			id,
		firstName				=			first_name,
		lastName				=			last_name,
		email					=			email,
		bio						=			bio,
		imageName				=			image_name,
		DOB						=			date_of_birth,
		hash					=			hash,
		emailToken				=			email_token,
		emailTokenExpiresAt		=			email_token_expires_at,
		emailVerifiedAt			=			email_verified_at,
		passwordToken			=			password_token,
		passwordTokenExpiresAt	=			password_token_expires_at,
		createdAt				=			created_at
	FROM tb_user 
	WHERE 
        CASE 
            WHEN @type = 'email' THEN email_token 
            WHEN @type = 'password' THEN password_token 
        END = @token
GO

CREATE PROCEDURE pr_user_create_password_token
	@id INT,
	@password_token VARCHAR(250)
AS
		UPDATE tb_user
		SET 
			password_token = @password_token,
			password_token_expires_at = DATEADD(DAY, 1, GETDATE())
		WHERE
			id = @id
GO

CREATE PROCEDURE pr_user_verify_email
	@user_id INT
AS
	UPDATE tb_user
	SET email_verified_at = GETDATE()
	WHERE id = @user_id
GO

CREATE PROCEDURE pr_user_change_password
	@user_id INT, 
	@new_hash VARCHAR(250)
AS
	UPDATE tb_user
	SET hash = @new_hash
	WHERE id = @user_id
GO

CREATE PROCEDURE pr_user_get_profile
	@source_id INT,
	@target_id INT
AS
	DECLARE @areFriends BIT;
	SELECT 
		@areFriends = (CASE WHEN COUNT(*) = 0 THEN 0 ELSE 1 END)
	FROM tb_friendship
	WHERE
		(source_id = @source_id OR target_id = @source_id)
		AND (source_id = target_id OR target_id = @target_id)
		AND status = 'accepted'

	SELECT 
		*,
		areFriends = @areFriends 
	FROM tb_user
	WHERE 
		id = @target_id
GO

CREATE PROCEDURE pr_user_get_by_id
	@user_id INT
AS
	SELECT 
		*, 
		totalFriends	=	(
								SELECT COUNT(*) FROM tb_friendship 
								WHERE 
									(source_id = @user_id OR target_id = @user_id)
									AND status = 'accepted'
							)
	FROM tb_user
	WHERE 
		id = @user_id
GO

CREATE PROCEDURE pr_user_get_by_email
	@email VARCHAR(250)
AS
	SELECT 
		id						=			id,
		firstName				=			first_name,
		lastName				=			last_name,
		email					=			email,
		bio						=			bio,
		imageName				=			image_name,
		DOB						=			date_of_birth,
		hash					=			hash,
		emailToken				=			email_token,
		emailTokenExpiresAt		=			email_token_expires_at,
		emailVerifiedAt			=			email_verified_at,
		passwordToken			=			password_token,
		passwordTokenExpiresAt	=			password_token_expires_at,
		createdAt				=			created_at
	FROM tb_user
	WHERE email = @email
GO

CREATE PROCEDURE pr_user_search		-- for friendship invitation
	@user_id INT,
	@username VARCHAR(250)
AS
	SELECT 
		*,
		areFriends		=	CASE WHEN F.source_id IS NULL THEN 0 ELSE 1 END
	FROM 
		tb_user U
	LEFT JOIN (
		SELECT * FROM tb_friendship 
		WHERE 
			(source_id = @user_id OR target_id = @user_id)
			AND status = 'accepted'
	) F
	ON U.id = F.source_id OR U.id = F.target_id
	WHERE 
		CONCAT(REPLACE(first_name, ' ', ''), REPLACE(last_name, ' ', '')) LIKE '%' + REPLACE(@username, ' ', '') + '%' 
GO

-- friendship
CREATE PROCEDURE pr_friendship_send_request
	@source_id INT,
	@target_id INT
AS
	INSERT INTO tb_friendship (source_id, target_id)
	VALUES (@source_id, @target_id)
GO

CREATE PROCEDURE pr_friendship_get_contacts
	@user_id INT,
	@status VARCHAR(250)							
AS
	-- my contacts
	IF @status = 'accepted'
		BEGIN
			SELECT 
				id						=			U.id,
				firstName				=			U.first_name,
				lastName				=			U.last_name,
				email					=			U.email,
				bio						=			U.bio,
				imageName				=			U.image_name,
				DOB						=			U.date_of_birth,
				hash					=			U.hash,
				emailToken				=			U.email_token,
				emailTokenExpiresAt		=			U.email_token_expires_at,
				emailVerifiedAt			=			U.email_verified_at,
				passwordToken			=			U.password_token,
				passwordTokenExpiresAt	=			U.password_token_expires_at,
				createdAt				=			U.created_at
			FROM tb_friendship F
			JOIN tb_user U ON U.id = F.source_id OR U.id = F.target_id
			WHERE 
				(source_id = @user_id OR target_id = @user_id) 
				AND status = @status 
				AND U.id != @user_id
		END
	-- my pending response
	ELSE
		BEGIN
			SELECT 
				id						=			F.source_id,
				firstName				=			U.first_name,
				lastName				=			U.last_name,
				email					=			U.email,
				bio						=			U.bio,
				imageName				=			U.image_name,
				DOB						=			U.date_of_birth,
				hash					=			U.hash,
				emailToken				=			U.email_token,
				emailTokenExpiresAt		=			U.email_token_expires_at,
				emailVerifiedAt			=			U.email_verified_at,
				passwordToken			=			U.password_token,
				passwordTokenExpiresAt	=			U.password_token_expires_at,
				createdAt				=			U.created_at
			FROM 
				tb_friendship F
			JOIN tb_user U ON U.id = F.source_id
			WHERE 
				status = 'pending'
				AND	F.target_id = @user_id
		END
GO

CREATE PROCEDURE pr_friendship_update_request
	@source_id INT,								-- the user_id who made the previous request
	@target_id INT,								-- the user_id that will accept or deny the request
	@status VARCHAR(250)
AS
	UPDATE tb_friendship
	SET 
		status = @status,
		updated_at = GETDATE()
	WHERE
		source_id = @source_id AND target_id = @target_id
GO

CREATE PROCEDURE pr_friendship_check_status
	@source_id INT,
	@user_id INT
AS
	SELECT 
		DISTINCT 
		arefriends		=	CASE WHEN my_friends.source_id IS NULL THEN 0 ELSE 1 END
	FROM
	(
		SELECT * FROM tb_friendship
		WHERE
			(source_id = @source_id OR target_id = @source_id) 
			AND	status = 'accepted'
	) my_friends
	WHERE
		my_friends.source_id = @user_id OR my_friends.target_id = @user_id
GO

-- POSTS 
CREATE PROCEDURE pr_post_upsert
	@user_id INT,
	@post_id INT = NULL,
	@message VARCHAR(500) = NULL,
	@media_type VARCHAR(250) = NULL,
	@file_name VARCHAR(250) = NULL
AS
	IF @post_id IS NULL
		BEGIN
			INSERT INTO tb_post (user_id, message, media_type, file_name)
			VALUES (@user_id, @message, @media_type, @file_name)
		END
	ELSE
		BEGIN
			UPDATE tb_post
				SET
					message = @message,
					media_type = @media_type,
					file_name = @file_name,
					updated_at = GETDATE()
			WHERE 
				id = @post_id 
				AND	user_id = @user_id
		END
GO

CREATE PROCEDURE pr_post_delete
	@post_id INT
AS
	DELETE FROM tb_post WHERE id = @post_id
GO

CREATE PROCEDURE pr_post_get_feed_posts
	@user_id INT
AS
	SELECT 
		DISTINCT
		id					=		P.id,
		userID				=		P.user_id,
		message				=		P.message,
		mediaType			=		P.media_type,
		fileName			=		P.file_name,
		isActive			=		P.is_active,
		updatedAt			=		P.updated_at,
		createdAt			=		P.created_at,
		reactionID			=		(SELECT id FROM tb_reaction WHERE user_id = @user_id AND type = 'post' AND entity_id = P.id),
		totalLikes			=		(SELECT COUNT(*) FROM tb_reaction WHERE type = 'post' AND entity_id = P.id),
		userLastLike		=		(	
										SELECT TOP 1 CONCAT(first_name, ' ', last_name) 
										FROM tb_reaction L 
										JOIN tb_user U ON U.id = L.user_id 
										WHERE type = 'post' AND entity_id = P.id
										ORDER BY L.created_at DESC
									),
		totalComments		=		(SELECT COUNT(*) FROM tb_reaction WHERE type = 'comment' AND entity_id = P.id),
		userLastComment		=		(
										SELECT TOP 1 CONCAT(first_name, ' ', last_name) 
										FROM tb_comment C 
										JOIN tb_user U ON U.id = C.user_id
										WHERE post_id = P.id ORDER BY C.created_at DESC
									)
	FROM tb_post P
	WHERE P.user_id IN
	(
		SELECT @user_id
		UNION
		SELECT
			CASE WHEN source_id = @user_id THEN target_id ELSE source_id END
		FROM tb_friendship
		WHERE
			(source_id = @user_id OR target_id = @user_id)
			AND status = 'accepted'
	)
	ORDER BY 
		P.created_at DESC
GO

CREATE PROCEDURE pr_post_get_by_id 
	@post_id INT
AS
	SELECT 
		id					=		id,
		userID				=		user_id,
		message				=		message,
		mediaType			=		media_type,
		fileName			=		file_name,
		isActive			=		is_active,
		updatedAt			=		updated_at,
		createdAt			=		created_at
	FROM tb_post
	WHERE 
		id = @post_id
GO

CREATE PROCEDURE pr_post_get_user_profile_posts
	@user_id INT
AS
	SELECT 
		id					=		P.id,
		userID				=		P.user_id,
		message				=		P.message,
		mediaType			=		P.media_type,
		fileName			=		P.file_name,
		isActive			=		P.is_active,
		updatedAt			=		P.updated_at,
		createdAt			=		P.created_at,
		reactionID			=		(SELECT id FROM tb_reaction WHERE user_id = @user_id AND type = 'post' AND entity_id = P.id),
		totalLikes			=		(SELECT COUNT(*) FROM tb_reaction WHERE type = 'post' AND entity_id = P.id),
		userLastLike		=		(	
										SELECT TOP 1 CONCAT(first_name, ' ', last_name) 
										FROM tb_reaction L 
										JOIN tb_user U ON U.id = L.user_id 
										WHERE type = 'post' AND entity_id = P.id
										ORDER BY L.created_at DESC
									),
		totalComments		=		(SELECT COUNT(*) FROM tb_reaction WHERE type = 'comment' AND entity_id = P.id),
		userLastComment		=		(
										SELECT TOP 1 CONCAT(first_name, ' ', last_name) 
										FROM tb_comment C 
										JOIN tb_user U ON U.id = C.user_id
										WHERE post_id = P.id 
										ORDER BY C.created_at DESC
									)
	FROM tb_post P
	WHERE 
		P.user_id = @user_id
	ORDER BY 
		P.created_at DESC
GO

-- REACTIONS
CREATE PROCEDURE pr_reaction_dislike
	@reaction_id INT
AS
	IF EXISTS(SELECT 1 FROM tb_reaction WHERE id = @reaction_id)
		DELETE FROM tb_reaction
		WHERE id = @reaction_id
GO

CREATE PROCEDURE pr_reaction_like
	@entity_id INT,
	@user_id INT,
	@type VARCHAR(250)				-- comment | post
AS

	IF NOT EXISTS(SELECT 1 FROM tb_reaction WHERE entity_id = @entity_id AND user_id = @user_id AND type = @type)
		INSERT INTO tb_reaction (user_id, entity_id, type)
		VALUES (@user_id, @entity_id, @type)
GO

-- COMMENT
CREATE PROCEDURE pr_comment_create
	@post_id INT,
	@user_id INT,
	@comment VARCHAR(500) = NULL,
	@media_type VARCHAR(250) = NULL,
	@file_name VARCHAR(250) = NULL
AS
	BEGIN
		INSERT INTO tb_comment (post_id, user_id, comment, media_type, file_name)
		VALUES 
			(@post_id, @user_id, @comment, @media_type, @file_name)
	END
GO

CREATE PROCEDURE pr_comment_update
	@comment_id INT = NULL,			-- @id = NULL ? CREATE : EDIT
	@comment VARCHAR(500) = NULL,
	@media_type VARCHAR(250) = NULL,
	@file_name VARCHAR(250) = NULL
AS
	UPDATE C
	SET
		comment = @comment,
		media_type = @media_type,
		file_name = @file_name,
		updated_at = GETDATE()
	FROM
		tb_comment C
	WHERE
		id = @comment_id
GO

CREATE PROCEDURE pr_comment_get_all
	@post_id INT	
AS
	SELECT 
		id				=		id,
		postID			=		post_id,
		userID			=		user_id,
		comment			=		comment,
		mediaType		=		media_type,
		fileName		=		file_name,
		isActive		=		is_active,
		createdAt		=		created_at,
		updatedAt		=		updated_at,
		totalLikes		=		(SELECT COUNT(*) FROM tb_reaction WHERE type = 'comment' AND entity_id = C.id)
	FROM tb_comment C
	WHERE
		post_id = @post_id
	ORDER BY 
		created_at DESC
GO

CREATE PROCEDURE pr_comment_delete
	@comment_id INT
AS
	DELETE FROM tb_comment WHERE id = @comment_id
GO

CREATE PROCEDURE pr_comment_get_by_id
	@comment_id INT
AS
	SELECT 
		id				=		id,
		postID			=		post_id,
		userID			=		user_id,
		comment			=		comment,
		mediaType		=		media_type,
		fileName		=		file_name,
		isActive		=		is_active,
		createdAt		=		created_at,
		updatedAt		=		updated_at,
		totalLikes		=		(SELECT COUNT(*) FROM tb_reaction WHERE type = 'comment' AND entity_id = C.id)
	FROM tb_comment C
	WHERE
		id = @comment_id

