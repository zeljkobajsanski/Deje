package rs.zeks.deje.activities;

import java.io.IOException;

import com.loopj.android.image.SmartImageView;

import rs.zeks.deje.Constants;
import rs.zeks.deje.R;
import rs.zeks.deje.R.id;
import rs.zeks.deje.model.Dobavljac;
import rs.zeks.deje.services.ServiceProxy;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Bundle;
import android.app.Activity;
import android.content.Intent;
import android.view.Menu;
import android.view.View;
import android.widget.Button;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

public class PregledDobavljacaActivity extends Activity {

	private ProgressBar progressBar;
	private SmartImageView slika;
	private TextView naziv;
	private TextView vrsta;
	private TextView opis;
	private TextView mesto;
	private Button telefon;
	private TextView www;
	private Dobavljac dobavljac;
	private Button btnPonuda;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_pregled_dobavljaca);
		progressBar = (ProgressBar)findViewById(id.progressBar1);
		progressBar.setIndeterminate(true);
		slika = (SmartImageView)findViewById(R.id.slikaDobavljaca);
		naziv = (TextView)findViewById(id.nazivDobavljaca);
		vrsta = (TextView)findViewById(id.vrstaDobavljaca);
		opis = (TextView)findViewById(id.opisDobavljaca);
		mesto = (TextView)findViewById(id.mesto);
		telefon = (Button) findViewById(id.btnTelefon);
		www = (Button) findViewById(id.btnWww);
		btnPonuda = (Button)findViewById(id.btnPonuda);
		Intent intent = getIntent();
		Integer id =  Integer.valueOf(intent.getIntExtra(Constants.ID_DOBAVLJACA, 0));
		new UcitajDobavljacaTask().execute(id);
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.activity_pregled_dobavljaca, menu);
		return true;
	}
	
	public void nazovi(View view) {
		Intent intent = new Intent(Intent.ACTION_CALL, Uri.parse("tel:" + dobavljac.getTelefon()));
		startActivity(intent);
	}
	
	public void pogledajSajt(View view) {
		Intent intent = new Intent(Intent.ACTION_VIEW, Uri.parse(dobavljac.getWww()));
		startActivity(intent);
	}
	
	public void prikaziPonudu(View view) {
		Intent intent = new Intent(this, PonudaDobavljacaActivity.class);
		intent.putExtra(Constants.ID_DOBAVLJACA, dobavljac.getId());
		startActivity(intent);
	}
	
	class UcitajDobavljacaTask extends AsyncTask<Integer, Void, Dobavljac> {

		private Exception exception;
		
		@Override
		protected void onPreExecute() {
			PregledDobavljacaActivity.this.progressBar.setVisibility(View.VISIBLE);
		}
		
		@Override
		protected Dobavljac doInBackground(Integer... arg0) {
			try {
				PregledDobavljacaActivity.this.dobavljac = new ServiceProxy().ucitajDobavljaca(arg0[0], false);
				return PregledDobavljacaActivity.this.dobavljac;
			} catch (IOException e) {
				exception = e;
				return null;
			}
			
		}
		
		@Override
		protected void onPostExecute(Dobavljac result) {
			if (result != null) {
				if (result.getSlika() != "null") {
					PregledDobavljacaActivity.this.slika.setImageUrl(result.getSlika());
				} else {
					PregledDobavljacaActivity.this.slika.setImageResource(R.drawable.photo_missing);
				}
				PregledDobavljacaActivity.this.naziv.setText(dobavljac.getNaziv());
				PregledDobavljacaActivity.this.vrsta.setText(dobavljac.getVrsta());
				PregledDobavljacaActivity.this.opis.setText(dobavljac.getOpis());
				PregledDobavljacaActivity.this.mesto.setText(dobavljac.getMesto());
				
				if (dobavljac.getTelefon() != "null") {
					PregledDobavljacaActivity.this.telefon.setVisibility(View.VISIBLE);
				}
				PregledDobavljacaActivity.this.telefon.setText("Tel: " + dobavljac.getTelefon());
				if (dobavljac.getWww() != "null") {
					PregledDobavljacaActivity.this.www.setVisibility(View.VISIBLE);
				}
				PregledDobavljacaActivity.this.www.setText(dobavljac.getWww());
				PregledDobavljacaActivity.this.btnPonuda.setVisibility(View.VISIBLE);
			}
			
			PregledDobavljacaActivity.this.progressBar.setVisibility(View.INVISIBLE);
			if (exception != null) {
				Toast.makeText(PregledDobavljacaActivity.this, "Greška prilikom pretrage. Proverite konekciju", Toast.LENGTH_LONG).show();
			}
		}
	}

}
