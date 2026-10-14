-- إعادة ضبط عداد الـ ID ليبدأ من رقم 2
DBCC CHECKIDENT ('Users', RESEED, 2);