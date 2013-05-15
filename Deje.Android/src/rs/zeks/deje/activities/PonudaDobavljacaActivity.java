package rs.zeks.deje.activities;

import java.io.IOException;
import java.util.List;

import com.loopj.android.image.SmartImageView;

import rs.zeks.deje.Constants;
import rs.zeks.deje.R;
import rs.zeks.deje.model.Artikal;
import rs.zeks.deje.model.Dobavljac;
import rs.zeks.deje.services.ServiceProxy;
import android.os.AsyncTask;
import android.os.Bundle;
import android.app.ListActivity;
import android.content.Context;
import android.view.Menu;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

public class PonudaDobavljacaActivity extends ListActivity {

	private Dobavljac dobavljac;
	private ProgressBar progressBar;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_ponuda_dobavljaca);
		int id = Integer.valueOf(getIntent().getIntExtra(Constants.ID_DOBAVLJACA, 0));
		progressBar = (ProgressBar)findViewById(R.id.progressBar1);
		progressBar.setIndeterminate(true);
		new UcitajDobavljacaTask().execute(id);
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.activity_ponuda_dobavljaca, menu);
		return true;
	}
	
	class UcitajDobavljacaTask extends AsyncTask<Integer, Void, Dobavljac> {

		private Exception exception;
		
		@Override
		protected void onPreExecute() {
			progressBar.setVisibility(View.VISIBLE);
		}

		@Override
		protected Dobavljac doInBackground(Integer... arg0) {
			try {
				return new ServiceProxy().ucitajDobavljaca(arg0[0], true);
			} catch (IOException e) {
				this.exception = e;
				return null;
			}
		}

		@Override
		protected void onPostExecute(Dobavljac result) {
			if (result != null) {
				PonudaDobavljacaActivity.this.dobavljac = result;
				PonudaDobavljacaActivity.this.setListAdapter(new PonudaAdapter(PonudaDobavljacaActivity.this, dobavljac.getPonuda()));
			}
			progressBar.setVisibility(View.INVISIBLE);
			if (exception != null) {
				Toast.makeText(PonudaDobavljacaActivity.this, "Greška prilikom pretrage. Proverite konekciju", Toast.LENGTH_LONG).show();
			}
		}
		
	}
	
	class PonudaAdapter extends ArrayAdapter<Artikal> {

		public PonudaAdapter(Context context, List<Artikal> objects) {
			super(context, R.layout.row_ponuda_dobavljaca, R.id.nazivArtikla, objects);
		}

		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			View row = convertView;
			Artikal artikal = dobavljac.getPonuda().get(position);
			if (row == null) {
				row = getLayoutInflater().inflate(R.layout.row_ponuda_dobavljaca, parent, false);
			}
			
			TextView kategorija = (TextView)row.findViewById(R.id.kategorija);
			kategorija.setText(artikal.getKategorija());
			if (position > 0) {
				Artikal prethodni = dobavljac.getPonuda().get(position - 1);
				boolean istaKategorija = prethodni.getKategorija().equals(artikal.getKategorija());
				kategorija.setVisibility(istaKategorija ? View.GONE : View.VISIBLE);
			} else {
				kategorija.setVisibility(View.VISIBLE);
			}
			
			SmartImageView slika = (SmartImageView) row.findViewById(R.id.slika);
			if (artikal.getSlika() != "null") {
				slika.setImageUrl(artikal.getSlika());
			} else {
				slika.setImageResource(R.drawable.photo_missing);
			}
			TextView naziv = (TextView)row.findViewById(R.id.nazivArtikla);
			naziv.setText(artikal.getNazivArtikla());
			TextView opis = (TextView)row.findViewById(R.id.opis);
			opis.setText(artikal.getOpis());
			TextView cena = (TextView)row.findViewById(R.id.cena);
			cena.setText(artikal.getCena());
			return row;
		}
		
		
		
	}

}
