package rs.zeks.deje;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.ArrayList;
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

public class ServiceAgent {
	
	public List<Artikal> ucitajArtiklePoNazivu(String latituda, String longituda, String udaljenost, String naziv) {
		String url = "http://mdeje.azurewebsites.net/home/PretraziArtiklePoNazivu?latituda=" + latituda +
				"&longituda=" + longituda + "&razdaljina=" + udaljenost + "&naziv=" + naziv;
		String json = getJson(url);
		List<Artikal> artikli = new ArrayList<Artikal>();
		try {
			JSONArray jsonArray = new JSONArray(json);
			for(int i = 0; i < jsonArray.length(); i++) {
				JSONObject jsonObject = jsonArray.getJSONObject(i);
				Artikal artikal = new Artikal();
				artikal.setId(jsonObject.getInt("Id"));
				artikal.setNazivArtikla(jsonObject.getString("Naziv"));
				artikal.setOpis(jsonObject.getString("Opis"));
				artikal.setSlika(jsonObject.getString("Slika"));
				artikli.add(artikal);
			}
		} catch (JSONException e) {
			e.printStackTrace();
		}
		return artikli;
	}
	
	private String getJson(String url) {
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
	      e.printStackTrace();
	    } catch (IOException e) {
	      e.printStackTrace();
	    }
	    return builder.toString();
	}
}
