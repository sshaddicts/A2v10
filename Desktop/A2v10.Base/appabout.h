// Copyright © 2008-2017 Alex Kukhtin. All rights reserved.

#pragma once

#undef AFX_DATA
#define AFX_DATA AFX_BASE_DATA

class CAppAboutDialog : public CDialogEx
{
	CImageList m_imageList;
public:
	CAppAboutDialog(CWnd* pParent = NULL);   // standard constructor
	virtual ~CAppAboutDialog();

// Dialog Data
	enum { IDD = IDD_ABOUT };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();

	DECLARE_MESSAGE_MAP()

	afx_msg void OnPaint();
};

#undef AFX_DATA
#define AFX_DATA
