-- MySQL dump 10.13  Distrib 8.0.11, for Win64 (x86_64)
--
-- Host: localhost    Database: admin_chatapp
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
  "created" datetime NOT NULL,
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
INSERT INTO "dbfiles" VALUES (107,'ic_launcherArtboard 1hdpi.png',52,'2019-05-01 10:03:13',120),(108,'ic_launcherArtboard 1hdpi.png',52,'2019-05-01 11:56:36',122),(109,'ARD.ai',52,'2019-05-01 11:57:12',123),(110,'ARD.jpg',52,'2019-05-01 11:59:16',124),(111,'ru.vkorotenko.bottom.apk',52,'2019-05-01 11:59:42',125),(112,'archive.xml',52,'2019-05-01 12:22:17',126),(113,'Трек-восьмой (2).doc',52,'2019-05-01 12:39:28',127),(114,'Трек-восьмой.doc',52,'2019-05-01 12:39:40',128),(115,'tightvnc-2.8.11-gpl-setup-64bit.msi',52,'2019-05-01 12:40:16',129),(116,'cuteftp.exe',52,'2019-05-01 12:41:52',130),(117,'Трек-восьмой (2).doc',52,'2019-05-01 12:42:22',132),(118,'ARD.jpg',52,'2019-05-01 12:42:57',133);
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
  "created" datetime NOT NULL,
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
INSERT INTO "dbmessages" VALUES (118,'проверка','2019-05-01 10:02:37',52,1,52),(119,'Проверка 2','2019-05-01 10:02:58',52,1,53),(120,'Проверка загрузки','2019-05-01 10:03:13',52,1,53),(121,'проверка','2019-05-01 10:28:18',52,1,53),(122,'','2019-05-01 11:56:36',52,1,53),(123,'','2019-05-01 11:57:12',52,1,53),(124,'','2019-05-01 11:59:16',52,1,52),(125,'','2019-05-01 11:59:42',52,1,52),(126,'','2019-05-01 12:22:16',52,1,53),(127,'','2019-05-01 12:39:28',52,1,53),(128,'','2019-05-01 12:39:40',52,1,53),(129,'','2019-05-01 12:40:15',52,1,53),(130,'','2019-05-01 12:41:51',52,1,53),(131,'','2019-05-01 12:42:07',52,1,53),(132,'','2019-05-01 12:42:22',52,1,53),(133,'','2019-05-01 12:42:57',52,1,53);
/*!40000 ALTER TABLE "dbmessages" ENABLE KEYS */;
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
  "created" datetime NOT NULL,
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
INSERT INTO "dbtopics" VALUES (52,'Тестовое объявление',52,'1A Auto','33332',52,'2019-05-01 10:02:25'),(53,'Капот для ВАЗ',53,'Стрелка 11','Капот',52,'2019-05-01 10:02:48');
/*!40000 ALTER TABLE "dbtopics" ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-05-01 18:34:38
