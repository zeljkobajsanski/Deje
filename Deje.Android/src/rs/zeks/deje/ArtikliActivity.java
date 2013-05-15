package rs.zeks.deje;

import java.util.List;

import com.loopj.android.image.SmartImageView;

import rs.zeks.deje.R.id;

import android.os.AsyncTask;
import android.os.Bundle;
import android.app.ListActivity;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ProgressBar;
import android.widget.TextView;

public class ArtikliActivity extends ListActivity {

	ProgressBar progressBar;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_artikli);
		String latituda = getIntent().getStringExtra("Latituda");
		String longituda = getIntent().getStringExtra("Longituda");
		String udaljenost = getIntent().getStringExtra("Udaljenost");
		String nazivArtikla = getIntent().getStringExtra("NazivArtikla");
		
		progressBar = (ProgressBar)findViewById(R.id.progressBar1);
		progressBar.setIndeterminate(true);
		
		new UcitajArtikleTask().execute(new String[]{latituda, longituda, udaljenost, nazivArtikla});
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.activity_artikli, menu);
		return true;
	}
	
	class ArtikliArrayAdapter extends ArrayAdapter<Artikal> {

		public ArtikliArrayAdapter(Context context) {
			super(context, rs.zeks.deje.R.layout.artikal_row, id.naziv);
		}

		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			View row = convertView;
			if (row == null) {
				LayoutInflater inflater = getLayoutInflater();
				row = inflater.inflate(rs.zeks.deje.R.layout.artikal_row, parent, false);
			}
			Artikal artikal = (Artikal)getItem(position);
			SmartImageView slika = (SmartImageView)row.findViewById(id.slika);
			slika.setImageUrl(artikal.getSlika());
			TextView naziv = (TextView)row.findViewById(id.naziv);
			naziv.setText(artikal.getNazivArtikla());
			TextView opis = (TextView)row.findViewById(id.opis);
			opis.setText(artikal.getOpis());
			return row;
		}
		
		
		
	}
	
	class UcitajArtikleTask extends AsyncTask<String, Void, List<Artikal>> {

		@Override
		protected void onPreExecute() {
			progressBar.setVisibility(View.VISIBLE);
		}

		@Override
		protected List<Artikal> doInBackground(String... arg0) {
			ServiceAgent agent = new ServiceAgent();
			List<Artikal> artikli = agent.ucitajArtiklePoNazivu(arg0[0], arg0[1], arg0[2], arg0[3]);
			return artikli;
		}

		@Override
		protected void onPostExecute(List<Artikal> result) {
			ArtikliArrayAdapter adapter = new ArtikliArrayAdapter(ArtikliActivity.this);
			for(Artikal artikal : result) {
				adapter.add(artikal);
			}
			ArtikliActivity.this.setListAdapter(adapter);
			progressBar.setVisibility(View.INVISIBLE);
		}
		
		
		
	}
}
