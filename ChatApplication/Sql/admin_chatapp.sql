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
/*!40000 ALTER TABLE "dbtopics" ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-04-30 11:34:39
