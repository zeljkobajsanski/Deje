package rs.zeks.deje.model;

import java.util.ArrayList;
import java.util.List;

public class Dobavljac {

	private int id;
	private double latituda;
	private double longituda;
	private String naziv;
	private String opis;
	private String udaljenost;
	private String vrsta;
	private String mesto;
	private String slika;
	private String telefon;
	private String www;
	private List<Artikal> ponuda = new ArrayList<Artikal>();
	
	public int getId() {
		return id;
	}
	public void setId(int id) {
		this.id = id;
	}
	public double getLatituda() {
		return latituda;
	}
	public void setLatituda(double latituda) {
		this.latituda = latituda;
	}
	public double getLongituda() {
		return longituda;
	}
	public void setLongituda(double longituda) {
		this.longituda = longituda;
	}
	public String getNaziv() {
		return naziv;
	}
	public void setNaziv(String naziv) {
		this.naziv = naziv;
	}
	public String getOpis() {
		return opis;
	}
	public void setOpis(String opis) {
		this.opis = opis;
	}
	public String getUdaljenost() {
		return udaljenost;
	}
	public void setUdaljenost(String udaljenost) {
		this.udaljenost = udaljenost;
	}
	public String getVrsta() {
		return vrsta;
	}
	public void setVrsta(String vrsta) {
		this.vrsta = vrsta;
	}
	
	public String getMesto() {
		return mesto;
	}
	public void setMesto(String mesto) {
		this.mesto = mesto;
	}
	public String getSlika() {
		return slika;
	}
	public void setSlika(String slika) {
		this.slika = slika;
	}
	public String getTelefon() {
		return telefon;
	}
	public void setTelefon(String telefon) {
		this.telefon = telefon;
	}
	public String getWww() {
		return www;
	}
	public void setWww(String www) {
		this.www = www;
	}
	
	public List<Artikal> getPonuda() {
		return ponuda;
	}
	
	public void DodajArtikal(Artikal artikal) {
		ponuda.add(artikal);
	}
	
	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + id;
		return result;
	}
	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		Dobavljac other = (Dobavljac) obj;
		if (id != other.id)
			return false;
		return true;
	}
	
	
	
}
