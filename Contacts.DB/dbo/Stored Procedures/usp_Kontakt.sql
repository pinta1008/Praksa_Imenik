CREATE PROC usp_Kontakt(@Id INT, @Ime NVARCHAR(100), @Prezime NVARCHAR(100), @Tel_broj INT,@Mail NVARCHAR(100), @Datum_kre NVARCHAR(100), @Vrijeme_kre NVARCHAR(100), @Datum_prom NVARCHAR(100), @Tip NVARCHAR(100))
AS
BEGIN
	
	if @Tip = 'SELECT'
	BEGIN
	SELECT * FROM Kontakti1;
	END;
	if @Tip = 'INSERT'
	BEGIN
	INSERT INTO Kontakti1(Ime, Prezime, Tel_broj, Mail, Datum_kre, Vrijeme_kre, Datum_prom) VALUES(@Ime, @Prezime, @Tel_broj,@Mail , @Datum_kre, @Vrijeme_kre, @Datum_prom);
	END;
	if @Tip = 'UPDATE'
	BEGIN
	UPDATE Kontakti1 SET Ime = @Ime, Prezime = @Prezime, Tel_broj = @Tel_broj, Mail = @Mail, Datum_kre = @Datum_kre, Vrijeme_kre = @Vrijeme_kre, Datum_prom = @Datum_prom WHERE Id = @Id;
	END;
	if @Tip = 'DELETE'
	BEGIN
	DELETE FROM Kontakti1 WHERE Id = @Id;
	END;



END;