package rs.zeks.deje.model;

public class Artikal {
	private int id;
	private String nazivArtikla;
	private String opis;
	private String slika;
	private String tip;
	private String kategorija;
	private String cena;
	
	public int getId() {
		return id;
	}
	public void setId(int id) {
		this.id = id;
	}
	public String getNazivArtikla() {
		return nazivArtikla;
	}
	public void setNazivArtikla(String nazivArtikla) {
		this.nazivArtikla = nazivArtikla;
	}
	public String getOpis() {
		return opis;
	}
	public void setOpis(String opis) {
		this.opis = opis;
	}
	public String getSlika() {
		return slika;
	}
	public void setSlika(String slika) {
		this.slika = slika;
	}
	public String getTip() {
		return tip;
	}
	public void setTip(String tip) {
		this.tip = tip;
	}
	public String getKategorija() {
		return kategorija;
	}
	public void setKategorija(String kategorija) {
		this.kategorija = kategorija;
	}
	public String getCena() {
		return "Cena: " + cena;
	}
	public void setCena(String cena) {
		this.cena = cena;
	}
	
	
	
}
