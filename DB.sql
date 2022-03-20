--
-- PostgreSQL database dump
--

-- Dumped from database version 10.15
-- Dumped by pg_dump version 10.15

-- Started on 2021-04-23 23:54:48

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 1 (class 3079 OID 12924)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2803 (class 0 OID 0)
-- Dependencies: 1
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 196 (class 1259 OID 17674)
-- Name: accounts; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.accounts (
    id integer NOT NULL,
    login text NOT NULL,
    password text NOT NULL,
    player_name text,
    player_exp integer,
    player_kills integer,
    player_deaths integer,
    player_cash integer,
    player_gold integer,
    bag1_primary integer,
    bag1_secondary integer,
    bag1_melee integer,
    bag1_grenade integer
);

INSERT INTO "public"."accounts" VALUES (1, 'test', '123', 'Test', 123, 123, 123, 123, 123, 1, 2, 4, 4);


ALTER TABLE public.accounts OWNER TO postgres;

--
-- TOC entry 197 (class 1259 OID 17680)
-- Name: player_inventory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.player_inventory (
    id integer NOT NULL,
    item_id integer NOT NULL,
    player_id integer NOT NULL
);

INSERT INTO "public"."player_inventory" VALUES (1, 1, 1);
INSERT INTO "public"."player_inventory" VALUES (2, 2, 1);
INSERT INTO "public"."player_inventory" VALUES (3, 3, 1);
INSERT INTO "public"."player_inventory" VALUES (4, 4, 1);

ALTER TABLE public.player_inventory OWNER TO postgres;

CREATE TABLE "public"."player_characters" (
  "id" int4 NOT NULL,
  "item_id" int4 NOT NULL,
  "player_id" int4 NOT NULL
);

INSERT INTO "public"."player_characters" VALUES (1, 1, 1);


