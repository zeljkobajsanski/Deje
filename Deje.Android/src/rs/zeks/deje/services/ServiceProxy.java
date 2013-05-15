package rs.zeks.deje.services;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.URLEncoder;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.StatusLine;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.DefaultHttpClient;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.util.Log;
import rs.zeks.deje.model.Adresa;
import rs.zeks.deje.model.Artikal;
import rs.zeks.deje.model.Dobavljac;
import rs.zeks.deje.model.GrupaArtikla;

public class ServiceProxy {
	
	private String serviceAddress = "http://deje1.azurewebsites.net";
	//private String serviceAddress = "http://192.168.1.5/deje.web.mobile";
	
	public List<Artikal> ucitajArtiklePoNazivu(String latituda, String longituda, String udaljenost, String naziv) throws IOException {
		List<Artikal> artikli = new ArrayList<Artikal>();
		try {
			String url = serviceAddress + "/Home/PretraziArtiklePoNazivu?latituda=" + latituda +
				"&longituda=" + longituda + "&razdaljina=" + udaljenost + "&naziv=" + URLEncoder.encode(naziv, "ISO-8859-1");
			JSONArray jsonArray = getJsonArray(url);
			for(int i = 0; i < jsonArray.length(); i++) {
				JSONObject jsonObject = jsonArray.getJSONObject(i);
				Artikal artikal = new Artikal();
				artikal.setId(jsonObject.getInt("Id"));
				artikal.setNazivArtikla(jsonObject.getString("Naziv"));
				artikal.setOpis(jsonObject.getString("Opis"));
				artikal.setSlika(jsonObject.getString("Slika"));
				artikal.setTip(jsonObject.getString("Tip"));
				artikli.add(artikal);
			}
		} catch (JSONException e) {
			Log.e("ServiceProxy.ucitajArtiklePoNazivu", e.getMessage());
		}
		return artikli;
	}
	
	public List<Artikal> ucitajArtiklePoGrupi(String latituda, String longituda,
			String udaljenost, String idGrupeArtikala) throws IOException {
		// http://mdeje.azurewebsites.net/Home/PretraziArtikle?idGrupeArtikala=63&latituda=45.2748463&longituda=19.8302276&razdaljina=2000
		String url = serviceAddress + "/Home/PretraziArtikle?latituda=" + latituda +
				"&longituda=" + longituda + "&razdaljina=" + udaljenost + "&idGrupeArtikala=" + idGrupeArtikala;
		List<Artikal> artikli = new ArrayList<Artikal>();
		try {
			JSONArray jsonArray = getJsonArray(url);
			for(int i = 0; i < jsonArray.length(); i++) {
				JSONObject jsonObject = jsonArray.getJSONObject(i);
				Artikal artikal = new Artikal();
				artikal.setId(jsonObject.getInt("Id"));
				artikal.setNazivArtikla(jsonObject.getString("Naziv"));
				artikal.setOpis(jsonObject.getString("Opis"));
				artikal.setSlika(jsonObject.getString("Slika"));
				artikal.setTip(jsonObject.getString("Tip"));
				artikli.add(artikal);
			}
		} catch (JSONException e) {
			Log.e("ServiceProxy.ucitajArtikalPoGrupi", e.getMessage());
		}
		return artikli;
	}
	
	public List<GrupaArtikla> pretraziGrupeArtikala(String latituda, String longituda, String udaljenost) throws IOException {
		//http://mdeje.azurewebsites.net/Home/Pronadji?latituda=45.2749142&longituda=19.8301853&udaljenost=2000&delatnost=1
		String url = serviceAddress + "/Home/Pronadji?latituda=" + latituda +
				"&longituda=" + longituda + "&udaljenost=" + udaljenost + "&delatnost=1";
		List<GrupaArtikla> grupe = new ArrayList<GrupaArtikla>();
		try{
			JSONObject json = getJsonObject(url);
			JSONArray nested = json.getJSONArray("grupeArtikala");
			for(int i = 0; i < nested.length(); i++) {
				JSONObject obj = nested.getJSONObject(i);
				GrupaArtikla g = new GrupaArtikla();
				g.setId(obj.getInt("id"));
				g.setNaziv(obj.getString("naziv"));
				grupe.add(g);
			}
		} catch(JSONException e) {
			Log.e("ServiceProxy.pretraziGrupeArtikala", e.getMessage());
		}
		return grupe;
	}
	
	public Collection<Dobavljac> pretraziDobavljace(String latituda, String longituda, String udaljenost, String id, String tip) throws IOException {
		//http://mdeje.azurewebsites.net/Home/PretraziDobavljace?latitude=45.248571899999995&longitude=19.876177&distance=2000&idArtikla=10&tip=s
		String url = serviceAddress + "/Home/PretraziDobavljace?latitude=" + latituda +
				"&longitude=" + longituda + "&distance=" + udaljenost + "&idArtikla=" + id + "&tip=" + tip;
		List<Dobavljac> dobavljaci = new ArrayList<Dobavljac>();
		try {
			JSONArray json = getJsonArray(url);
			for(int i = 0; i < json.length(); i++) {
				JSONObject obj = json.getJSONObject(i);
				Dobavljac dobavljac = new Dobavljac();
				dobavljac.setId(obj.getInt("Id"));
				dobavljac.setLatituda(obj.getDouble("Latitude"));
				dobavljac.setLongituda(obj.getDouble("Longitude"));
				dobavljac.setNaziv(obj.getString("Naziv"));
				dobavljac.setOpis(obj.getString("Opis"));
				dobavljac.setUdaljenost(obj.getString("Udaljenost"));
				dobavljac.setVrsta(obj.getString("Vrsta"));
				if (!dobavljaci.contains(dobavljac)) {
					dobavljaci.add(dobavljac);	
				}
			}
		} catch (JSONException e) {
			Log.e("ServiceProxy.pretraziDobavljace", e.getMessage());
		}
		return dobavljaci;
	}
	
	public Dobavljac ucitajDobavljaca(int id, boolean ucitajPonudu) throws IOException {
		// http://mdeje.azurewebsites.net/Home/VratiDobavljaca?id=59
		String url = serviceAddress + "/Home/VratiDobavljaca?id=" + id;
		Dobavljac dobavljac = new Dobavljac();
		try {
			JSONObject json = getJsonObject(url);
			
			dobavljac.setId(json.getInt("Id"));
			dobavljac.setMesto(json.getString("Mesto"));
			dobavljac.setNaziv(json.getString("Naziv"));
			dobavljac.setOpis(json.getString("Opis"));
			dobavljac.setSlika(json.getString("Slika"));
			dobavljac.setTelefon(json.getString("Telefon"));
			dobavljac.setVrsta(json.getString("VrstaDobavljaca"));
			dobavljac.setWww(json.getString("Www"));
			if (!ucitajPonudu) return dobavljac;
			JSONArray ponuda = json.getJSONArray("Ponuda");
			for(int i = 0; i < ponuda.length(); i++) {
				JSONObject kategorija = ponuda.getJSONObject(i);
				JSONArray artikli = kategorija.getJSONArray("Artikli");
				for(int j = 0; j < artikli.length(); j++) {
					JSONObject artikal = artikli.getJSONObject(j);
					Artikal a = new Artikal();
					a.setKategorija(kategorija.getString("Kategorija"));
					a.setSlika(artikal.getString("Slika"));
					a.setNazivArtikla(artikal.getString("Naziv"));
					a.setOpis(artikal.getString("Opis"));
					a.setCena(artikal.getString("Cena"));
					dobavljac.DodajArtikal(a);
				}
			}
		} catch (JSONException e) {
			Log.e("ServiceProxy.ucitajDobavljaca", e.getMessage());
		}
		return dobavljac;
		
	}
	
	public List<Adresa> pronadjiAdrese(String adresa) {
		List<Adresa> adrese = new ArrayList<Adresa>();
		try {
			String url = "http://maps.googleapis.com/maps/api/geocode/json?sensor=true&address=" + URLEncoder.encode(adresa, "UTF8");
			JSONObject json = getJsonObject(url);
			String status = json.getString("status");
			if (!status.equals("OK")) return adrese;
			JSONArray resultsArray = json.getJSONArray("results");
			for(int i = 0; i < resultsArray.length(); i++) {
				JSONObject jsonObj = resultsArray.getJSONObject(i);
				Adresa a = new Adresa(jsonObj.getString("formatted_address"));
				JSONObject geometry = jsonObj.getJSONObject("geometry");
				JSONObject location = geometry.getJSONObject("location");
				a.setLatituda(location.getDouble("lat"));
				a.setLongituda(location.getDouble("lng"));
				adrese.add(a);
			}
			
		} catch (Exception e) {
			Log.e("ServiceProxy.pronadjiAdrese", e.getMessage());
		}
		return adrese;
	}
	
	private JSONArray getJsonArray(String url) throws JSONException, IOException {
		return new JSONArray(getJson(url));
	}
	
	private JSONObject getJsonObject(String url) throws JSONException, IOException {
		String json = getJson(url);
		return new JSONObject(json);
	}
	
	private String getJson(String url) throws IOException {
		StringBuilder builder = new StringBuilder();
	    HttpClient client = new DefaultHttpClient();
	    HttpGet httpGet = new HttpGet(url);
	    try {
	      HttpResponse response = client.execute(httpGet);
	      StatusLine statusLine = response.getStatusLine();
	      int statusCode = statusLine.getStatusCode();
	      if (statusCode == 200) {
	        HttpEntity entity = response.getEntity();
	        InputStream content = entity.getContent();
	        BufferedReader reader = new BufferedReader(new InputStreamReader(content));
	        String line;
	        while ((line = reader.readLine()) != null) {
	          builder.append(line);
	        }
	      } else {
	        
	      }
	    } catch (ClientProtocolException e) {
	      Log.e("ServiceProxy.getJson", e.getMessage());
	    } 
	    return builder.toString();
	}

	
}
