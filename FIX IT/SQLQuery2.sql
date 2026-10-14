-- هذا الأمر يعيد ضبط الترقيم التلقائي لجدول Categories ليبدأ من الصفر.
DBCC CHECKIDENT ('dbo.Categories', RESEED, 0);