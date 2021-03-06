// Copyright � 2008-2019 Alex Kukhtin. All rights reserved.

#pragma once


#define IDR_MAINFRAME		128
#define IDR_JSMODULE        129
#define IDR_FORM			130
#define IDR_DEBUG			131

#define IDR_MENU_IMAGES		230
#define IDR_TOOLBOX         232
#define IDR_PROPERTIES      234
#define IDR_PRINTPREVIEW    236
#define IDR_WND_CONSOLE		238
#define IDR_SOLUTION		240

#define ID_WND_PROPERTIES   500
#define ID_WND_TOOLBOX		501
#define ID_WND_COMMAND      502
#define ID_WND_SOLUTION     503
#define ID_WND_OUTLINE      504
#define ID_WND_CONSOLE		505

#define IDM_POPUP_MENU		600
#define IDM_POPUP_FORM		601
#define IDM_POPUP_SOLUTION  602
#define IDM_POPUP_COMMAND_INDEX  0
#define IDM_POPUP_JSEDIT_INDEX   1
#define IDM_POPUP_XAMLEDIT_INDEX 2
#define IDM_POPUP_CONSOLE_INDEX  3
#define IDM_POPUP_SOLUTION_ADD_INDEX 0
#define IDM_POPUP_APPTOOLS	603


#define IDIL_10X10				700
#define IDIL_12x12				701
#define IDIL_48X48				702

#define IDD_OPTION_GENERAL  800
#define IDD_ABOUT			801
#define IDD_SOLUTION_CREATE 802
#define IDD_DB_LOGIN		803

#define IDS_OPTIONS_CAPTION  12000
#define IDS_LANGUAGES        12001
#define IDS_TOOLBAR_STANDARD 12002
#define IDS_TOOLBAR_DEBUG	 12003

#define IDC_DIALOG_FIRST      14000
#define IDC_COMBO1			  14001
#define IDC_COMBO2			  14002
#define IDC_COMBO3			  14003
#define IDC_COMBO4			  14004
#define IDC_COMBO5			  14005
#define IDC_TITLE1			  14006
#define IDC_TITLE2			  14007
#define IDC_TITLE3			  14008
#define IDC_TITLE4			  14009
#define IDC_TITLE5			  14010
#define IDC_PROMPT1			  14011
#define IDC_PROMPT2			  14012
#define IDC_PROMPT3			  14013
#define IDC_PROMPT4			  14014
#define IDC_PROMPT5			  14015
#define IDC_TEXT1			14016
#define IDC_TEXT2			14017
#define IDC_TEXT3			14018
#define IDC_TEXT4			14019
#define IDC_TEXT5			14020
#define IDC_RADIO1			14021
#define IDC_RADIO2			14022
#define IDC_RADIO3			14023
#define IDC_RADIO4			14024
#define IDC_RADIO5			14025
#define IDC_CHECK1			14026
#define IDC_CHECK2			14027
#define IDC_CHECK3			14028
#define IDC_CHECK4			14029
#define IDC_CHECK5			14030


#define IDS_ID_APP_ABOUT		17000
#define IDS_ID_TOOLS_OPTIONS	17001
#define IDS_ID_SHOW_DEVTOOLS	17002

#define IDS_LOADING				18000

#define IDS_WEB_RESOURCE_FIRST    20000
#define IDS_WEB_RESOURCE_LAST     20999

// file: 32000
#define ID_FILE_OPEN_SOLUTION		32000
#define ID_FILE_CLOSE_SOLUTION		32001
#define ID_FILE_SAVE_SOLUTION		32002
#define ID_FILE_NEW_SOLUTION		32003
#define ID_FILE_SAVE_ALL			32004
#define ID_APP_START				32005
#define ID_APP_LOAD					32006
// edit: 32100
#define ID_EDIT_FIND_IN_FILES	32100
// browse: 32150
#define ID_NAVIGATE_BACK		32150
#define ID_NAVIGATE_FORWARD		32151
#define ID_NAVIGATE_REFRESH		32152
#define ID_NAVIGATE_REFRESH_IGNORE_CACHE 32153
#define ID_SHOW_DEVTOOLS		32154
#define ID_APP_TOOLS			32155
// view: 32200
#define ID_VIEW_PROPERTIES		32200
#define	ID_VIEW_TOOLBOX			32201
#define ID_VIEW_COMMAND			32202
#define ID_VIEW_SOLUTION		32203
#define ID_VIEW_OUTLINE			32204
#define ID_VIEW_CONSOLE			32205
// project & debug: 32300
#define ID_DEBUG_RUN			32300
#define ID_DEBUG_RUN_INT        32301
#define ID_DEBUG_STEP_INTO		32302
#define ID_DEBUG_STEP_OVER		32303
#define ID_DEBUG_STEP_OUT		32304
// tools: 32400
#define ID_TOOLS_OPTIONS	32400
// toolbox: 32500
#define ID_TOOLBOX_FIRST    32500
#define ID_TOOLBOX_POINTER  32500
#define ID_TOOLBOX_LABEL    32501
#define ID_TOOLBOX_TEXTBOX  32502
#define ID_TOOLBOX_BUTTON   32503
#define ID_TOOLBOX_CHECK    32504
#define ID_TOOLBOX_RADIO	32505
#define ID_TOOLBOX_COMBOBOX	32506
#define ID_TOOLBOX_DATAGRID 32507
#define ID_TOOLBOX_DATEPICKER 32508
#define ID_TOOLBOX_CANVAS	32509
#define ID_TOOLBOX_GRID		32510
#define ID_TOOLBOX_DOCKPANEL	32511
#define ID_TOOLBOX_STACKPANEL	32512
#define ID_TOOLBOX_LAST     32512

// window:32600
#define ID_WINDOW_MANAGER	32600
#define ID_WINDOW_CLOSE_ALL 32601

// other:32700
#define ID_PROP_ALPHABETICAL   32700
#define ID_PROP_CATEGORIZED    32701
#define ID_SOLUTION_ADD_TABLE  32702
#define ID_SOLUTION_ADD_VIEW   32703
#define ID_SOLUTION_ADD_COLUMN 32704
#define ID_SOLUTION_ADD		   32729

// indicators:32800
#define ID_INDICATOR_LNCOL   32800

#define IDP_OLE_INIT_FAILED	 51000
#define IDP_RICH_INIT_FAILED 51001
#define IDP_SCI_INIT_FAILED	 51002


#define IDMENU_CLOSE    0xD000
#define IDMENU_RESTORE  0xD001
#define IDMENU_MAXIMIZE 0xD002
#define IDMENU_MINIMIZE 0xD003
#define IDMENU_HELP     0xD004
#define IDMENU_BACK     0xD005
#define IDMENU_FORWARD  0xD006
#define IDMENU_RELOAD   0xD007
#define IDMENU_TOOLS    0xD008

#define AFX_IDW_TOOLBAR2                 0xE807 
#define AFX_IDW_TOOLBAR3                 0xE808 

#define IDW_GAPBAR_LEFT   (AFX_IDW_CONTROLBAR_LAST - 1)
#define IDW_GAPBAR_TOP    (AFX_IDW_CONTROLBAR_LAST - 2)
#define IDW_GAPBAR_RIGHT  (AFX_IDW_CONTROLBAR_LAST - 3)
#define IDW_GAPBAR_BOTTOM (AFX_IDW_CONTROLBAR_LAST - 4)
