USE [master]
GO
/****** Object:  Database [demobadminton]    Script Date: 7/22/2024 12:22:49 AM ******/
CREATE DATABASE [demobadminton]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'demobadminton', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\demobadminton.mdf' , SIZE = 3264KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'demobadminton_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\demobadminton_log.ldf' , SIZE = 816KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [demobadminton] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [demobadminton].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [demobadminton] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [demobadminton] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [demobadminton] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [demobadminton] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [demobadminton] SET ARITHABORT OFF 
GO
ALTER DATABASE [demobadminton] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [demobadminton] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [demobadminton] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [demobadminton] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [demobadminton] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [demobadminton] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [demobadminton] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [demobadminton] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [demobadminton] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [demobadminton] SET  ENABLE_BROKER 
GO
ALTER DATABASE [demobadminton] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [demobadminton] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [demobadminton] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [demobadminton] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [demobadminton] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [demobadminton] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [demobadminton] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [demobadminton] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [demobadminton] SET  MULTI_USER 
GO
ALTER DATABASE [demobadminton] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [demobadminton] SET DB_CHAINING OFF 
GO
ALTER DATABASE [demobadminton] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [demobadminton] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [demobadminton] SET DELAYED_DURABILITY = DISABLED 
GO
USE [demobadminton]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 7/22/2024 12:22:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 7/22/2024 12:22:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 7/22/2024 12:22:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 7/22/2024 12:22:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 7/22/2024 12:22:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 7/22/2024 12:22:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 7/22/2024 12:22:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 7/22/2024 12:22:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Booking]    Script Date: 7/22/2024 12:22:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Booking](
	[B_ID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[CO_ID] [int] NOT NULL,
	[B_Guest_Name] [nvarchar](100) NULL,
	[B_Booking_Type] [nvarchar](100) NOT NULL,
	[B_Total_Hours] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[B_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Court]    Script Date: 7/22/2024 12:22:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Court](
	[CO_ID] [int] IDENTITY(1,1) NOT NULL,
	[CO_Name] [nvarchar](255) NOT NULL,
	[CO_Path] [nvarchar](255) NULL,
	[CO_Status] [bit] NOT NULL,
	[CO_Address] [nvarchar](255) NOT NULL,
	[CO_Info] [nvarchar](1000) NOT NULL,
	[CO_Price] [float] NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[CO_BeneficiaryAccountName] [nvarchar](50) NULL,
	[CO_BeneficiaryPayPalEmail] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[CO_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CourtCondition]    Script Date: 7/22/2024 12:22:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourtCondition](
	[CD_ID] [int] IDENTITY(1,1) NOT NULL,
	[CD_CreatedAt] [datetime] NOT NULL,
	[CD_SurfaceCondition] [int] NOT NULL,
	[CD_NetCondition] [int] NOT NULL,
	[CD_LightningCondition] [int] NOT NULL,
	[CD_CleanlinessCondition] [int] NOT NULL,
	[CD_OverallCondition] [int] NOT NULL,
	[CD_Notes] [nvarchar](max) NULL,
	[CO_ID] [int] NOT NULL,
 CONSTRAINT [PK_CourtCondition] PRIMARY KEY CLUSTERED 
(
	[CD_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Payment]    Script Date: 7/22/2024 12:22:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[P_ID] [int] IDENTITY(1,1) NOT NULL,
	[P_Amount] [decimal](11, 2) NOT NULL,
	[P_Date_Time] [datetime] NOT NULL,
	[B_ID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[P_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Rating]    Script Date: 7/22/2024 12:22:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rating](
	[RatingId] [int] IDENTITY(1,1) NOT NULL,
	[CourtId] [int] NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[Rating] [int] NULL,
	[Review] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[RatingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TimeSlot]    Script Date: 7/22/2024 12:22:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TimeSlot](
	[TS_ID] [int] IDENTITY(1,1) NOT NULL,
	[TS_Date] [date] NOT NULL,
	[TS_Start] [time](7) NOT NULL,
	[TS_End] [time](7) NOT NULL,
	[TS_Checked_in] [bit] NOT NULL,
	[CO_ID] [int] NOT NULL,
	[B_ID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserActiveStatus]    Script Date: 7/22/2024 12:22:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserActiveStatus](
	[Id] [nvarchar](450) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_UserActiveStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240615062515_inital', N'8.0.6')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'09d13fad-ba0c-4477-860d-fb87a45e215f', N'admin', N'ADMIN', NULL)
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'46b99b9e-e3a8-4a21-b277-673f19fefc56', N'Manager', N'MANAGER', NULL)
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'cb99a1de-4eb9-4e11-a62b-a4c506f334e2', N'Staff', N'STAFF', NULL)
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'22519f87-ebfe-4ce0-a95d-68f39b5c1205', N'09d13fad-ba0c-4477-860d-fb87a45e215f')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'7f9b448e-5caf-4c35-9c83-0af337dc617a', N'09d13fad-ba0c-4477-860d-fb87a45e215f')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'1', N'46b99b9e-e3a8-4a21-b277-673f19fefc56')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'7f9b448e-5caf-4c35-9c83-0af337dc617a', N'cb99a1de-4eb9-4e11-a62b-a4c506f334e2')
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'1', N'maxyz', N'MAXYZ', N'truongvi1511@gmail.com', N'TRUONGVI1511@GMAIL.COM', 1, N'AQAAAAIAAYagAAAAEAYCJvw7y/oZsCTbpUtYjh4WUPogqwBRsQ3qEx14AZCCp2DOlerNSFO0RV/7YvC5Qw==', N'XM3UA2JWNTPVAXCVUQHAP6AIG5ALAXNW', N'b4da9bab-d40e-4f43-a8a8-0bf47e4ced91', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'12c02798-53f4-4dd2-8046-c2459198da69', N'theappleorange75@gmail.com', N'THEAPPLEORANGE75@GMAIL.COM', N'theappleorange75@gmail.com', N'THEAPPLEORANGE75@GMAIL.COM', 1, N'AQAAAAIAAYagAAAAEKZCQBgai4hKhp0eEAAzRmmJWc66uH6GMY77lSa4JvwYS7gePNFy4sMk5jtDPhIu4g==', N'7IMXF32Y5F7STX2IWJ3NMQFIAGAM2RVQ', N'5f5b6d8d-da8a-45e1-af95-e0226a07b279', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'22519f87-ebfe-4ce0-a95d-68f39b5c1205', N'Iam@darkin123', N'IAM@DARKIN123', N'Iam@darkin123', N'IAM@DARKIN123', 1, N'AQAAAAIAAYagAAAAEI94tsiZDUiF10pYYh/FsQYAGof+UM3RzzjDiWfvF9XrZR/2yNjpd8Wohk8FZAYR9g==', N'3EOVIXAKQVWMC3YLD3QMA2VUPNE67T7U', N'08b05002-62da-4f3d-94d6-062526fa1e3a', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'39b02517-b090-4878-8a34-0902b390198a', N'ditdse184543@fpt.edu.vn', N'DITDSE184543@FPT.EDU.VN', N'ditdse184543@fpt.edu.vn', N'DITDSE184543@FPT.EDU.VN', 1, NULL, N'6XDVQH3L36HUUZ36AGW7K6UF4QXG275Y', N'0b55b879-c934-49db-bbdb-4eb01b51d2f6', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'584a9db0-6402-4c4d-b9a0-bdb0f126e780', N'thejifpeanut69@gmail.com', N'THEJIFPEANUT69@GMAIL.COM', N'thejifpeanut69@gmail.com', N'THEJIFPEANUT69@GMAIL.COM', 1, N'AQAAAAIAAYagAAAAEEI5e//ghdUvqDp6Nj9lirJsbx6nsI8zq5xkoUZRIJu5/Fo2MOTrtrFT8dnzgiD38w==', N'4RY6VNLEW3VZP7CR2OWOTEAFZSTIDOVR', N'017d5d0b-ed86-454c-87b0-b386441468b4', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'7f9b448e-5caf-4c35-9c83-0af337dc617a', N'Nature lover', N'NATURE LOVER', N'nature_lover_19@example.com', N'NATURE_LOVER_19@EXAMPLE.COM', 1, N'AQAAAAIAAYagAAAAEKZCQBgai4hKhp0eEAAzRmmJWc66uH6GMY77lSa4JvwYS7gePNFy4sMk5jtDPhIu4g==', N'B5SUB4SKHJT4EJSFKDXTZ5YMCSCO7KB5', N'49eb3051-41c9-48d6-b7d6-3ddd58a8e196', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'8041b6ab-1b50-4005-8658-c978478f3193', N'Coding Master', N'CODING MASTER', N'coding_enthusiast_21@example.com', N'CODING_ENTHUSIAST_21@EXAMPLE.COM', 1, N'AQAAAAIAAYagAAAAEDEVTsNCsLCQptkz2T3i2IwlTfB1TeTIo5P39VJCnHWiI+qDgZM/MhfffWNHKzV1Cw==', N'YELXE37LFU32G42AXM4OW37RHOO7RYUZ', N'1b56dbff-b8c7-41b1-85c7-5c71e0f0593d', NULL, 0, 0, NULL, 1, 0)
SET IDENTITY_INSERT [dbo].[Booking] ON 

INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (1, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (2, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (3, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (4, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (5, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (6, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (7, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (8, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (9, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (10, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (11, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (12, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (13, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (14, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (15, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (16, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (17, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (18, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (19, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (20, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (21, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (22, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (23, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (24, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (25, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (26, N'1', 3, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (27, N'584a9db0-6402-4c4d-b9a0-bdb0f126e780', 1, NULL, N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (28, N'1', 1, N'truongvi1511@gmail.com', N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (29, N'1', 1, N'truongvi1511@gmail.com', N'Casual', NULL)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (30, N'22519f87-ebfe-4ce0-a95d-68f39b5c1205', 2, N'Iam@darkin123', N'Flexible', 0)
INSERT [dbo].[Booking] ([B_ID], [UserId], [CO_ID], [B_Guest_Name], [B_Booking_Type], [B_Total_Hours]) VALUES (31, N'1', 1, N'truongvi1511@gmail.com', N'Casual', 0)
SET IDENTITY_INSERT [dbo].[Booking] OFF
SET IDENTITY_INSERT [dbo].[Court] ON 

INSERT [dbo].[Court] ([CO_ID], [CO_Name], [CO_Path], [CO_Status], [CO_Address], [CO_Info], [CO_Price], [UserId], [CO_BeneficiaryAccountName], [CO_BeneficiaryPayPalEmail]) VALUES (1, N'Court A', N'b929d15a-7a6d-4e0f-ac0a-b5fba2b1e95d_court3.jpg', 0, N'Ho Chi Minh, District 1', N'Very deluxe badminton court', 50000, N'1', NULL, NULL)
INSERT [dbo].[Court] ([CO_ID], [CO_Name], [CO_Path], [CO_Status], [CO_Address], [CO_Info], [CO_Price], [UserId], [CO_BeneficiaryAccountName], [CO_BeneficiaryPayPalEmail]) VALUES (2, N'Court B', N'80b63ede-90f1-4c36-baf0-84f933de5a78_images (5).jpg', 1, N'Ho Chi Minh, District 1', N'adsad', 50000, N'1', NULL, NULL)
INSERT [dbo].[Court] ([CO_ID], [CO_Name], [CO_Path], [CO_Status], [CO_Address], [CO_Info], [CO_Price], [UserId], [CO_BeneficiaryAccountName], [CO_BeneficiaryPayPalEmail]) VALUES (3, N'Court C', N'e091b77e-72b2-498d-9395-0a5a0db7b3cf_nhanvien.jpg', 1, N'Ho Chi Minh, District 1', N'adsad', 50000, N'1', NULL, NULL)
INSERT [dbo].[Court] ([CO_ID], [CO_Name], [CO_Path], [CO_Status], [CO_Address], [CO_Info], [CO_Price], [UserId], [CO_BeneficiaryAccountName], [CO_BeneficiaryPayPalEmail]) VALUES (4, N'Court D', N'e4923922-76d8-4cef-8e01-5ebe8ae50792_court.jpg', 1, N'Ho Chi Minh, District 1', N'asfafasa', 14144, N'1', NULL, NULL)
INSERT [dbo].[Court] ([CO_ID], [CO_Name], [CO_Path], [CO_Status], [CO_Address], [CO_Info], [CO_Price], [UserId], [CO_BeneficiaryAccountName], [CO_BeneficiaryPayPalEmail]) VALUES (5, N'Court E', N'cace6768-e178-432a-a519-272f1376f84b_1708872251-SÂNCẦULÔNGNHƯNHIÊN.jpg', 1, N'Ho Chi Minh, District 1', N'vip badminton court', 100000, N'1', NULL, NULL)
INSERT [dbo].[Court] ([CO_ID], [CO_Name], [CO_Path], [CO_Status], [CO_Address], [CO_Info], [CO_Price], [UserId], [CO_BeneficiaryAccountName], [CO_BeneficiaryPayPalEmail]) VALUES (6, N'Court F', N'326f4cb1-423e-4b5d-afa0-a37d3aeff527_court2.jpg', 1, N'Ho Chi Minh, District 1', N'mvp court', 200000, N'1', NULL, NULL)
INSERT [dbo].[Court] ([CO_ID], [CO_Name], [CO_Path], [CO_Status], [CO_Address], [CO_Info], [CO_Price], [UserId], [CO_BeneficiaryAccountName], [CO_BeneficiaryPayPalEmail]) VALUES (7, N'Court G', N'fc65359b-ca85-40e1-a384-0866aa95e2fc_1689220970-SÂNCẦULÔNGSGBVƯỜNLÀI(3).jpg', 1, N'Ho Chi Minh, District 1', N'wide court', 70000, N'1', NULL, NULL)
INSERT [dbo].[Court] ([CO_ID], [CO_Name], [CO_Path], [CO_Status], [CO_Address], [CO_Info], [CO_Price], [UserId], [CO_BeneficiaryAccountName], [CO_BeneficiaryPayPalEmail]) VALUES (8, N'Court H', N'8af3268a-a740-4c63-a384-18faa5fe0dab_badmintoncourt.jpeg', 1, N'Ho Chi Minh, District 2', N'normal court', 50000, N'1', NULL, NULL)
INSERT [dbo].[Court] ([CO_ID], [CO_Name], [CO_Path], [CO_Status], [CO_Address], [CO_Info], [CO_Price], [UserId], [CO_BeneficiaryAccountName], [CO_BeneficiaryPayPalEmail]) VALUES (10, N'Court J', N'bf5d24cc-2634-44f4-8c5c-1b9e6f454543_court1.jpg', 1, N'Ho Chi Minh, District 3', N'asdasda', 145000, N'1', NULL, NULL)
INSERT [dbo].[Court] ([CO_ID], [CO_Name], [CO_Path], [CO_Status], [CO_Address], [CO_Info], [CO_Price], [UserId], [CO_BeneficiaryAccountName], [CO_BeneficiaryPayPalEmail]) VALUES (11, N'Court K', N'f4293741-7042-44c3-b41e-07d3b5fa80d8_nuocchanh.jpg', 1, N'Ho Chi Minh, District 3', N'asfasfasf', 55000, N'1', NULL, NULL)
INSERT [dbo].[Court] ([CO_ID], [CO_Name], [CO_Path], [CO_Status], [CO_Address], [CO_Info], [CO_Price], [UserId], [CO_BeneficiaryAccountName], [CO_BeneficiaryPayPalEmail]) VALUES (17, N'court vip pro', N'48bf326c-1c1f-4c81-86d7-e5f19d3142f4_court.jpg', 1, N'adasd', N'asdasd', 50000, N'1', NULL, NULL)
INSERT [dbo].[Court] ([CO_ID], [CO_Name], [CO_Path], [CO_Status], [CO_Address], [CO_Info], [CO_Price], [UserId], [CO_BeneficiaryAccountName], [CO_BeneficiaryPayPalEmail]) VALUES (18, N'Insane', N'813b9218-b33f-4f30-a3cb-f24b833cb1ba_court4.jpg', 0, N'Ho Chi Minh, District 3', N'san cau long tphcm', 33123, N'1', NULL, NULL)
INSERT [dbo].[Court] ([CO_ID], [CO_Name], [CO_Path], [CO_Status], [CO_Address], [CO_Info], [CO_Price], [UserId], [CO_BeneficiaryAccountName], [CO_BeneficiaryPayPalEmail]) VALUES (19, N'fghfh', N'a82969ef-35c4-4c7d-bbf5-5da32437beeb_court4.jpg', 0, N'Ho Chi Minh, District 1', N'san cau long vang dai', 70, N'1', NULL, NULL)
INSERT [dbo].[Court] ([CO_ID], [CO_Name], [CO_Path], [CO_Status], [CO_Address], [CO_Info], [CO_Price], [UserId], [CO_BeneficiaryAccountName], [CO_BeneficiaryPayPalEmail]) VALUES (20, N'fghfh', N'83db7f3a-6f21-4ed1-9160-92323ae47e8a_1689221427-SÂNCẦULÔNGSGBVƯỜNLÀI(3).jpg', 0, N'Ho Chi Minh, District 1', N'dsasdad', 80.5, N'1', NULL, NULL)
INSERT [dbo].[Court] ([CO_ID], [CO_Name], [CO_Path], [CO_Status], [CO_Address], [CO_Info], [CO_Price], [UserId], [CO_BeneficiaryAccountName], [CO_BeneficiaryPayPalEmail]) VALUES (21, N'court west', N'6d2717db-eb38-44ff-8bc7-05b6a1364ad7_tải xuống.jpg', 0, N'Ho Chi Minh, District 3', N'sadad', 14, N'1', NULL, NULL)
SET IDENTITY_INSERT [dbo].[Court] OFF
SET IDENTITY_INSERT [dbo].[CourtCondition] ON 

INSERT [dbo].[CourtCondition] ([CD_ID], [CD_CreatedAt], [CD_SurfaceCondition], [CD_NetCondition], [CD_LightningCondition], [CD_CleanlinessCondition], [CD_OverallCondition], [CD_Notes], [CO_ID]) VALUES (1, CAST(N'2024-07-11 02:48:52.663' AS DateTime), 5, 5, 5, 5, 5, NULL, 1)
INSERT [dbo].[CourtCondition] ([CD_ID], [CD_CreatedAt], [CD_SurfaceCondition], [CD_NetCondition], [CD_LightningCondition], [CD_CleanlinessCondition], [CD_OverallCondition], [CD_Notes], [CO_ID]) VALUES (2, CAST(N'2024-07-11 02:49:28.417' AS DateTime), 5, 5, 5, 5, 5, N'fdsfds', 2)
INSERT [dbo].[CourtCondition] ([CD_ID], [CD_CreatedAt], [CD_SurfaceCondition], [CD_NetCondition], [CD_LightningCondition], [CD_CleanlinessCondition], [CD_OverallCondition], [CD_Notes], [CO_ID]) VALUES (3, CAST(N'2024-07-11 03:32:22.327' AS DateTime), 5, 2, 3, 5, 4, N'Awesome', 1)
INSERT [dbo].[CourtCondition] ([CD_ID], [CD_CreatedAt], [CD_SurfaceCondition], [CD_NetCondition], [CD_LightningCondition], [CD_CleanlinessCondition], [CD_OverallCondition], [CD_Notes], [CO_ID]) VALUES (4, CAST(N'2024-07-11 04:01:30.327' AS DateTime), 5, 3, 5, 5, 5, NULL, 5)
INSERT [dbo].[CourtCondition] ([CD_ID], [CD_CreatedAt], [CD_SurfaceCondition], [CD_NetCondition], [CD_LightningCondition], [CD_CleanlinessCondition], [CD_OverallCondition], [CD_Notes], [CO_ID]) VALUES (5, CAST(N'2024-07-11 04:05:43.020' AS DateTime), 1, 3, 5, 3, 3, NULL, 7)
SET IDENTITY_INSERT [dbo].[CourtCondition] OFF
SET IDENTITY_INSERT [dbo].[Payment] ON 

INSERT [dbo].[Payment] ([P_ID], [P_Amount], [P_Date_Time], [B_ID]) VALUES (1, CAST(50000000.00 AS Decimal(11, 2)), CAST(N'2024-06-25 16:26:53.657' AS DateTime), 28)
INSERT [dbo].[Payment] ([P_ID], [P_Amount], [P_Date_Time], [B_ID]) VALUES (2, CAST(50000000.00 AS Decimal(11, 2)), CAST(N'2024-06-25 16:36:04.013' AS DateTime), 29)
INSERT [dbo].[Payment] ([P_ID], [P_Amount], [P_Date_Time], [B_ID]) VALUES (3, CAST(250000.00 AS Decimal(11, 2)), CAST(N'2024-07-08 15:05:04.830' AS DateTime), 30)
INSERT [dbo].[Payment] ([P_ID], [P_Amount], [P_Date_Time], [B_ID]) VALUES (4, CAST(200000.00 AS Decimal(11, 2)), CAST(N'2024-07-11 03:36:50.873' AS DateTime), 31)
SET IDENTITY_INSERT [dbo].[Payment] OFF
SET IDENTITY_INSERT [dbo].[Rating] ON 

INSERT [dbo].[Rating] ([RatingId], [CourtId], [UserId], [Rating], [Review], [CreatedAt]) VALUES (1, 1, N'1', 1, N'bad', CAST(N'2024-06-25 16:36:04.013' AS DateTime))
INSERT [dbo].[Rating] ([RatingId], [CourtId], [UserId], [Rating], [Review], [CreatedAt]) VALUES (2, 3, N'1', 2, N'good', CAST(N'2024-06-27 09:46:10.823' AS DateTime))
INSERT [dbo].[Rating] ([RatingId], [CourtId], [UserId], [Rating], [Review], [CreatedAt]) VALUES (3, 3, N'1', 4, N'very good', CAST(N'2024-06-27 13:47:06.150' AS DateTime))
INSERT [dbo].[Rating] ([RatingId], [CourtId], [UserId], [Rating], [Review], [CreatedAt]) VALUES (4, 1, N'1', 4, N'nice', CAST(N'2024-06-25 18:36:04.013' AS DateTime))
INSERT [dbo].[Rating] ([RatingId], [CourtId], [UserId], [Rating], [Review], [CreatedAt]) VALUES (5, 3, N'1', 4, NULL, CAST(N'2024-06-27 16:28:38.390' AS DateTime))
INSERT [dbo].[Rating] ([RatingId], [CourtId], [UserId], [Rating], [Review], [CreatedAt]) VALUES (6, 3, N'1', 5, N'pussy', CAST(N'2024-06-28 13:41:33.857' AS DateTime))
INSERT [dbo].[Rating] ([RatingId], [CourtId], [UserId], [Rating], [Review], [CreatedAt]) VALUES (7, 3, N'1', 4, N'pussy', CAST(N'2024-06-28 13:42:20.840' AS DateTime))
INSERT [dbo].[Rating] ([RatingId], [CourtId], [UserId], [Rating], [Review], [CreatedAt]) VALUES (8, 1, N'1', 5, N'pussy couort', CAST(N'2024-06-28 13:43:30.613' AS DateTime))
SET IDENTITY_INSERT [dbo].[Rating] OFF
SET IDENTITY_INSERT [dbo].[TimeSlot] ON 

INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (1, CAST(N'2024-06-19' AS Date), CAST(N'13:00:00' AS Time), CAST(N'14:00:00' AS Time), 1, 3, 1)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (2, CAST(N'2024-06-21' AS Date), CAST(N'15:00:00' AS Time), CAST(N'16:00:00' AS Time), 1, 3, 2)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (3, CAST(N'2024-06-22' AS Date), CAST(N'14:00:00' AS Time), CAST(N'15:00:00' AS Time), 1, 3, 3)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (4, CAST(N'2024-06-22' AS Date), CAST(N'15:00:00' AS Time), CAST(N'16:00:00' AS Time), 1, 3, 4)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (5, CAST(N'2024-06-18' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), 0, 3, 5)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (6, CAST(N'2024-06-20' AS Date), CAST(N'12:00:00' AS Time), CAST(N'13:00:00' AS Time), 0, 3, 6)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (7, CAST(N'2024-06-21' AS Date), CAST(N'17:00:00' AS Time), CAST(N'18:00:00' AS Time), 0, 3, 7)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (8, CAST(N'2024-06-18' AS Date), CAST(N'19:00:00' AS Time), CAST(N'20:00:00' AS Time), 0, 3, 8)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (9, CAST(N'2024-06-20' AS Date), CAST(N'20:00:00' AS Time), CAST(N'21:00:00' AS Time), 0, 3, 9)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (10, CAST(N'2024-06-19' AS Date), CAST(N'18:00:00' AS Time), CAST(N'19:00:00' AS Time), 0, 3, 10)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (11, CAST(N'2024-06-20' AS Date), CAST(N'18:00:00' AS Time), CAST(N'19:00:00' AS Time), 0, 3, 11)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (12, CAST(N'2024-06-20' AS Date), CAST(N'19:00:00' AS Time), CAST(N'20:00:00' AS Time), 0, 3, 12)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (13, CAST(N'2024-06-20' AS Date), CAST(N'17:00:00' AS Time), CAST(N'18:00:00' AS Time), 0, 3, 13)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (14, CAST(N'2024-06-21' AS Date), CAST(N'16:00:00' AS Time), CAST(N'17:00:00' AS Time), 0, 3, 14)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (15, CAST(N'2024-06-22' AS Date), CAST(N'16:00:00' AS Time), CAST(N'17:00:00' AS Time), 0, 3, 15)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (16, CAST(N'2024-06-22' AS Date), CAST(N'16:00:00' AS Time), CAST(N'17:00:00' AS Time), 0, 3, 16)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (17, CAST(N'2024-06-21' AS Date), CAST(N'16:00:00' AS Time), CAST(N'17:00:00' AS Time), 0, 3, 17)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (18, CAST(N'2024-06-20' AS Date), CAST(N'17:00:00' AS Time), CAST(N'18:00:00' AS Time), 0, 3, 18)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (19, CAST(N'2024-06-23' AS Date), CAST(N'18:00:00' AS Time), CAST(N'19:00:00' AS Time), 0, 3, 19)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (20, CAST(N'2024-06-23' AS Date), CAST(N'19:00:00' AS Time), CAST(N'20:00:00' AS Time), 0, 3, 20)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (21, CAST(N'2024-06-24' AS Date), CAST(N'19:00:00' AS Time), CAST(N'20:00:00' AS Time), 0, 3, 21)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (22, CAST(N'2024-06-24' AS Date), CAST(N'20:00:00' AS Time), CAST(N'21:00:00' AS Time), 0, 3, 22)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (23, CAST(N'2024-06-24' AS Date), CAST(N'17:00:00' AS Time), CAST(N'18:00:00' AS Time), 0, 3, 23)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (24, CAST(N'2024-06-24' AS Date), CAST(N'13:00:00' AS Time), CAST(N'14:00:00' AS Time), 0, 3, 24)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (25, CAST(N'2024-06-24' AS Date), CAST(N'12:00:00' AS Time), CAST(N'13:00:00' AS Time), 0, 3, 25)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (26, CAST(N'2024-06-24' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), 0, 3, 26)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (27, CAST(N'2024-06-22' AS Date), CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time), 1, 1, 27)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (28, CAST(N'2024-06-25' AS Date), CAST(N'09:00:00' AS Time), CAST(N'10:00:00' AS Time), 1, 1, 28)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (29, CAST(N'2024-07-01' AS Date), CAST(N'09:00:00' AS Time), CAST(N'10:00:00' AS Time), 1, 1, 29)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (30, CAST(N'2024-07-22' AS Date), CAST(N'20:00:00' AS Time), CAST(N'21:00:00' AS Time), 0, 2, 30)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (31, CAST(N'2024-07-22' AS Date), CAST(N'19:00:00' AS Time), CAST(N'20:00:00' AS Time), 0, 2, 30)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (32, CAST(N'2024-07-22' AS Date), CAST(N'18:00:00' AS Time), CAST(N'19:00:00' AS Time), 0, 2, 30)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (33, CAST(N'2024-07-22' AS Date), CAST(N'17:00:00' AS Time), CAST(N'18:00:00' AS Time), 0, 2, 30)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (34, CAST(N'2024-07-22' AS Date), CAST(N'16:00:00' AS Time), CAST(N'17:00:00' AS Time), 0, 2, 30)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (35, CAST(N'2024-07-17' AS Date), CAST(N'20:00:00' AS Time), CAST(N'21:00:00' AS Time), 0, 1, 31)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (36, CAST(N'2024-07-17' AS Date), CAST(N'19:00:00' AS Time), CAST(N'20:00:00' AS Time), 0, 1, 31)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (37, CAST(N'2024-07-17' AS Date), CAST(N'18:00:00' AS Time), CAST(N'19:00:00' AS Time), 0, 1, 31)
INSERT [dbo].[TimeSlot] ([TS_ID], [TS_Date], [TS_Start], [TS_End], [TS_Checked_in], [CO_ID], [B_ID]) VALUES (38, CAST(N'2024-07-17' AS Date), CAST(N'17:00:00' AS Time), CAST(N'18:00:00' AS Time), 0, 1, 31)
SET IDENTITY_INSERT [dbo].[TimeSlot] OFF
INSERT [dbo].[UserActiveStatus] ([Id], [IsActive]) VALUES (N'1', 1)
INSERT [dbo].[UserActiveStatus] ([Id], [IsActive]) VALUES (N'7f9b448e-5caf-4c35-9c83-0af337dc617a', 1)
INSERT [dbo].[UserActiveStatus] ([Id], [IsActive]) VALUES (N'8041b6ab-1b50-4005-8658-c978478f3193', 0)
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 7/22/2024 12:22:49 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [RoleNameIndex]    Script Date: 7/22/2024 12:22:49 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 7/22/2024 12:22:49 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 7/22/2024 12:22:49 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 7/22/2024 12:22:49 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [EmailIndex]    Script Date: 7/22/2024 12:22:49 AM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UserNameIndex]    Script Date: 7/22/2024 12:22:49 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Booking.CO_ID] FOREIGN KEY([CO_ID])
REFERENCES [dbo].[Court] ([CO_ID])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK_Booking.CO_ID]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Booking_AspNetUsers] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK_Booking_AspNetUsers]
GO
ALTER TABLE [dbo].[Court]  WITH CHECK ADD  CONSTRAINT [FK_Court_AspNetUsers] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Court] CHECK CONSTRAINT [FK_Court_AspNetUsers]
GO
ALTER TABLE [dbo].[CourtCondition]  WITH CHECK ADD  CONSTRAINT [FK_CourtCondition_Court] FOREIGN KEY([CO_ID])
REFERENCES [dbo].[Court] ([CO_ID])
GO
ALTER TABLE [dbo].[CourtCondition] CHECK CONSTRAINT [FK_CourtCondition_Court]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD  CONSTRAINT [FK_Payment.B_ID] FOREIGN KEY([B_ID])
REFERENCES [dbo].[Booking] ([B_ID])
GO
ALTER TABLE [dbo].[Payment] CHECK CONSTRAINT [FK_Payment.B_ID]
GO
ALTER TABLE [dbo].[Rating]  WITH CHECK ADD  CONSTRAINT [FK_Court_Rating] FOREIGN KEY([CourtId])
REFERENCES [dbo].[Court] ([CO_ID])
GO
ALTER TABLE [dbo].[Rating] CHECK CONSTRAINT [FK_Court_Rating]
GO
ALTER TABLE [dbo].[TimeSlot]  WITH CHECK ADD  CONSTRAINT [FK_Time Slot.B_ID] FOREIGN KEY([B_ID])
REFERENCES [dbo].[Booking] ([B_ID])
GO
ALTER TABLE [dbo].[TimeSlot] CHECK CONSTRAINT [FK_Time Slot.B_ID]
GO
ALTER TABLE [dbo].[TimeSlot]  WITH CHECK ADD  CONSTRAINT [FK_Time Slot.CO_ID] FOREIGN KEY([CO_ID])
REFERENCES [dbo].[Court] ([CO_ID])
GO
ALTER TABLE [dbo].[TimeSlot] CHECK CONSTRAINT [FK_Time Slot.CO_ID]
GO
ALTER TABLE [dbo].[UserActiveStatus]  WITH CHECK ADD  CONSTRAINT [FK_UserActiveStatus_AspNetUsers] FOREIGN KEY([Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[UserActiveStatus] CHECK CONSTRAINT [FK_UserActiveStatus_AspNetUsers]
GO
USE [master]
GO
ALTER DATABASE [demobadminton] SET  READ_WRITE 
GO
