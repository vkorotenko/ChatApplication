-- MySQL dump 10.13  Distrib 8.0.11, for Win64 (x86_64)
--
-- Host: localhost    Database: chatapp
-- ------------------------------------------------------
-- Server version	8.0.11
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO,ANSI' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table "dbfiles"
--

DROP TABLE IF EXISTS "dbfiles";
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE "dbfiles" (
  "id" bigint(20) NOT NULL AUTO_INCREMENT,
  "name" varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  "authorid" int(11) NOT NULL,
  "created" datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  "messageid" bigint(20) NOT NULL,
  PRIMARY KEY ("id"),
  KEY "IX_AuthorID" ("authorid"),
  KEY "IX_MessageID" ("messageid"),
  KEY "IX_Created" ("created")
);
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table "dbfiles"
--

LOCK TABLES "dbfiles" WRITE;
/*!40000 ALTER TABLE "dbfiles" DISABLE KEYS */;
INSERT INTO "dbfiles" VALUES (99,'filetest.txt',52,'2019-01-01 22:23:00',77),(100,'uploads',3072,'2019-04-26 08:54:24',98),(101,'users.sql',577,'2019-04-26 09:11:55',100);
/*!40000 ALTER TABLE "dbfiles" ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table "dbmessages"
--

DROP TABLE IF EXISTS "dbmessages";
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE "dbmessages" (
  "id" bigint(20) NOT NULL AUTO_INCREMENT,
  "body" longtext NOT NULL,
  "created" datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  "authorid" int(11) NOT NULL,
  "isread" tinyint(4) NOT NULL DEFAULT '0',
  "topicid" bigint(20) NOT NULL,
  PRIMARY KEY ("id"),
  KEY "IX_Created" ("created"),
  KEY "IX_AuthorID" ("authorid"),
  KEY "IX_IsRead" ("isread"),
  KEY "IX_TopicID" ("topicid")
);
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table "dbmessages"
--

LOCK TABLES "dbmessages" WRITE;
/*!40000 ALTER TABLE "dbmessages" DISABLE KEYS */;
INSERT INTO "dbmessages" VALUES (77,'Проверочное сообщение','2019-01-01 22:23:00',52,0,52),(78,'Второе сообщение','2019-04-25 13:14:52',52,0,52),(79,'Третье сообщение ответ','2019-04-25 14:18:46',48,0,52),(80,'Четвертое сообщение от какого то чувака. Что то тут нетак с чатом','2019-04-25 14:24:09',49,0,52),(83,'test','2019-04-25 23:33:20',52,0,52),(84,'test3','2019-04-25 23:34:26',52,0,52),(85,'test4','2019-04-25 23:35:51',52,0,52),(86,'<h1> test</h1>','2019-04-25 23:40:56',52,0,52),(87,'Проверка  очистки формы','2019-04-25 23:42:34',52,0,52),(88,'Проверка подсветки автора','2019-04-25 23:44:40',52,0,52),(89,'Проверка на добавление аватара','2019-04-25 23:47:32',52,0,52);
/*!40000 ALTER TABLE "dbmessages" ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table "dbroles"
--

DROP TABLE IF EXISTS "dbroles";
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE "dbroles" (
  "id" int(11) NOT NULL AUTO_INCREMENT,
  "name" varchar(45) NOT NULL,
  PRIMARY KEY ("id")
);
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table "dbroles"
--

LOCK TABLES "dbroles" WRITE;
/*!40000 ALTER TABLE "dbroles" DISABLE KEYS */;
INSERT INTO "dbroles" VALUES (1,'admin'),(2,'user');
/*!40000 ALTER TABLE "dbroles" ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table "dbtopics"
--

DROP TABLE IF EXISTS "dbtopics";
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE "dbtopics" (
  "id" bigint(20) NOT NULL AUTO_INCREMENT,
  "title" varchar(200) NOT NULL,
  "announcementid" bigint(20) NOT NULL,
  "vendor" varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  "vendorcode" varchar(60) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  "authorid" int(11) NOT NULL,
  "created" datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY ("id"),
  KEY "IX_Created" ("created"),
  KEY "IX_AuthorID" ("authorid"),
  KEY "IX_AnoncementID" ("announcementid")
);
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table "dbtopics"
--

LOCK TABLES "dbtopics" WRITE;
/*!40000 ALTER TABLE "dbtopics" DISABLE KEYS */;
INSERT INTO "dbtopics" VALUES (52,'Тестовое обьявление',52,'TestVendor','TestVendorCode',52,'2019-01-01 22:23:00'),(53,'Капот для ВАЗ',53,'VAZ','Капот',52,'2019-04-25 09:48:50'),(60,'Тестик объяв12',60,'Тойота','Капот',52,'2019-04-25 09:50:31'),(61,'Тестик объяв234',61,'Камаз','Капот',52,'2019-04-25 09:50:31');
/*!40000 ALTER TABLE "dbtopics" ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table "dbuserinroles"
--

DROP TABLE IF EXISTS "dbuserinroles";
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE "dbuserinroles" (
  "id" bigint(20) NOT NULL AUTO_INCREMENT,
  "roleid" int(11) NOT NULL,
  "userid" int(11) NOT NULL,
  PRIMARY KEY ("id"),
  KEY "IX_RoleID" ("roleid"),
  KEY "IX_UserID" ("userid")
);
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table "dbuserinroles"
--

LOCK TABLES "dbuserinroles" WRITE;
/*!40000 ALTER TABLE "dbuserinroles" DISABLE KEYS */;
INSERT INTO "dbuserinroles" VALUES (1,1,52),(2,2,52);
/*!40000 ALTER TABLE "dbuserinroles" ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table "dbusers"
--

DROP TABLE IF EXISTS "dbusers";
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE "dbusers" (
  "id" int(11) NOT NULL,
  "username" varchar(60) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  "name" varchar(60) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  "lastname" varchar(60) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  "middlename" varchar(60) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  "email" varchar(200) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  "password" varchar(40) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  "lastactive" datetime DEFAULT NULL,
  PRIMARY KEY ("id"),
  KEY "IX_LastActive" ("lastactive")
);
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table "dbusers"
--

LOCK TABLES "dbusers" WRITE;
/*!40000 ALTER TABLE "dbusers" DISABLE KEYS */;
INSERT INTO "dbusers" VALUES (52,'79210843080','Дефолт','Дефолтов',NULL,'test@ya.ru','123','2019-04-23 07:18:33'),(54,'9103421220','petr','Ivanov','Ivanovitch','ivanov@test.com','123','2019-01-01 22:23:00'),(55,'79103421225','Vladimir','Korotenko','Nikolaevitch','koroten@ya.ru','123','2019-04-19 20:51:44'),(56,'79611759152','Владимир','Коротенко','Николаевич','koroten@ya.ru','123','2019-04-19 20:55:08'),(57,'79611796220','Модем','Модемов','Модемыч','test@ya.ru','123','2019-04-19 20:56:21');
/*!40000 ALTER TABLE "dbusers" ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-04-26 10:20:27
