-- إعادة العداد لصفر ليبدأ المستخدم القادم برقم 1
DBCC CHECKIDENT ('Users', RESEED, 0);