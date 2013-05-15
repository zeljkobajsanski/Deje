package rs.zeks.deje.model;

public class Adresa {
	private String adresa;
	private String mesto;
	private double latituda;
	private double longituda;
	
	public Adresa(String adresa) {
		super();
		this.adresa = adresa;
	}
	public Adresa(String adresa, double latituda, double longituda) {
		super();
		this.adresa = adresa;
		this.latituda = latituda;
		this.longituda = longituda;
	}
	public String getAdresa() {
		return adresa;
	}
	public void setAdresa(String adresa) {
		this.adresa = adresa;
	}
	public String getMesto() {
		return mesto;
	}
	public void setMesto(String mesto) {
		this.mesto = mesto;
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
	
	
}
