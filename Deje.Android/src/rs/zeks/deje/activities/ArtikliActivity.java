package rs.zeks.deje.activities;

import java.io.IOException;
import java.util.List;

import com.loopj.android.image.SmartImageView;

import rs.zeks.deje.Constants;
import rs.zeks.deje.R;
import rs.zeks.deje.R.id;
import rs.zeks.deje.model.Artikal;
import rs.zeks.deje.services.ServiceProxy;

import android.os.AsyncTask;
import android.os.Bundle;
import android.app.ListActivity;
import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

public class ArtikliActivity extends ListActivity {

	private ProgressBar progressBar;
	private List<Artikal> artikli;
	private String latituda;
	private String longituda;
	private String udaljenost;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_artikli);
		latituda = getIntent().getStringExtra(Constants.LATITUDA);
		longituda = getIntent().getStringExtra(Constants.LONGITUDA);
		udaljenost = getIntent().getStringExtra(Constants.UDALJENOST);
		String nazivArtikla = getIntent().getStringExtra(Constants.NAZIV_ARTIKLA);
		String idGrupeArtikla = getIntent().getStringExtra(Constants.ID_GRUPE_ARTIKLA);
		progressBar = (ProgressBar)findViewById(R.id.progressBar1);
		progressBar.setIndeterminate(true);
		getListView().setChoiceMode(ListView.CHOICE_MODE_SINGLE);
		new UcitajArtikleTask().execute(new String[]{latituda, longituda, udaljenost, nazivArtikla, idGrupeArtikla});
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.activity_artikli, menu);
		return true;
	}
	
	
	
	@Override
	protected void onListItemClick(ListView l, View v, int position, long id) {
		Artikal artikal = artikli.get(position);
		Intent intent = new Intent(this, DobavljaciActivity.class);
		intent.putExtra(Constants.LATITUDA, latituda);
		intent.putExtra(Constants.LONGITUDA, longituda);
		intent.putExtra(Constants.UDALJENOST, udaljenost);
		intent.putExtra(Constants.ID_ARTIKLA, String.valueOf(artikal.getId()));
		intent.putExtra(Constants.TIP_ARTIKLA, artikal.getTip());
		startActivity(intent);
	}



	class ArtikliArrayAdapter extends ArrayAdapter<Artikal> {

		public ArtikliArrayAdapter(Context context, List<Artikal> artikli) {
			super(context, rs.zeks.deje.R.layout.row_artikal, id.naziv, artikli);
		}

		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			View row = convertView;
			if (row == null) {
				LayoutInflater inflater = getLayoutInflater();
				row = inflater.inflate(rs.zeks.deje.R.layout.row_artikal, parent, false);
			}
			Artikal artikal = (Artikal)getItem(position);
			SmartImageView slika = (SmartImageView)row.findViewById(id.slika);
			String putanjaSlike = artikal.getSlika();
			if (putanjaSlike != "" && putanjaSlike != "null") {
				slika.setImageUrl(putanjaSlike);	
			} else {
				slika.setImageResource(R.drawable.photo_missing);
			}
			TextView naziv = (TextView)row.findViewById(id.naziv);
			naziv.setText(artikal.getNazivArtikla());
			TextView opis = (TextView)row.findViewById(id.opis);
			opis.setText(artikal.getOpis());
			return row;
		}
		
		
		
	}
	
	class UcitajArtikleTask extends AsyncTask<String, Void, List<Artikal>> {

		private Exception exception;
		@Override
		protected void onPreExecute() {
			progressBar.setVisibility(View.VISIBLE);
		}

		@Override
		protected List<Artikal> doInBackground(String... arg0) {
			ServiceProxy agent = new ServiceProxy();
			List<Artikal> artikli = null;
			if (arg0[3] != null) {
				try {
					artikli = agent.ucitajArtiklePoNazivu(arg0[0], arg0[1], arg0[2], arg0[3]);
				} catch (IOException e) {
					exception = e;
				}	
			}
			if (arg0[4] != null) {
				try {
					artikli = agent.ucitajArtiklePoGrupi(arg0[0], arg0[1], arg0[2], arg0[4]);
				} catch (IOException e) {
					exception = e;
				}
			}
			
			return artikli;
		}

		@Override
		protected void onPostExecute(List<Artikal> result) {
			if (result != null) {
				artikli = result;
				if (result.size() == 0) {
					Toast.makeText(ArtikliActivity.this, R.string.nema_rezultata, Toast.LENGTH_SHORT).show();
					progressBar.setVisibility(View.INVISIBLE);
					return;
				}
				ArtikliArrayAdapter adapter = new ArtikliArrayAdapter(ArtikliActivity.this, artikli);
				setListAdapter(adapter);
			}
			
			progressBar.setVisibility(View.INVISIBLE);
			if (exception != null) {
				Toast.makeText(ArtikliActivity.this, "Greška prilikom pretrage. Proverite konekciju", Toast.LENGTH_LONG).show();
			}
		}
		
		
		
	}
}
