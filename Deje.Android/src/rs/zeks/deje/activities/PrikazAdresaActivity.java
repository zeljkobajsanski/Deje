package rs.zeks.deje.activities;

import java.util.List;
import rs.zeks.deje.Constants;
import rs.zeks.deje.R;
import rs.zeks.deje.R.id;
import rs.zeks.deje.model.Adresa;
import rs.zeks.deje.services.ServiceProxy;
import android.os.AsyncTask;
import android.os.Bundle;
import android.app.ListActivity;
import android.content.Context;
import android.content.Intent;
import android.view.Menu;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

public class PrikazAdresaActivity extends ListActivity {

	private ProgressBar progressBar;
	private List<Adresa> adrese;
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_prikaz_adresa);
		progressBar = (ProgressBar)findViewById(R.id.progressBar1);
		String adresa = getIntent().getStringExtra(Constants.ADRESA);
		new PretragaAdresaTask().execute(adresa);
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.activity_prikaz_adresa, menu);
		return true;
	}
	
	
	
	@Override
	protected void onListItemClick(ListView l, View v, int position, long id) {
		Adresa adresa = adrese.get(position);
		String a = adresa.getAdresa();
		Intent intent = new Intent();
		intent.putExtra(Constants.ADRESA, a);
		intent.putExtra(Constants.LATITUDA, adresa.getLatituda());
		intent.putExtra(Constants.LONGITUDA, adresa.getLongituda());
		setResult(RESULT_OK, intent);
		finish();
	}



	class PretragaAdresaTask extends AsyncTask<String, Void, List<Adresa>> {

		@Override
		protected void onPreExecute() {
			PrikazAdresaActivity.this.progressBar.setVisibility(View.VISIBLE);
		}

		@Override
		protected List<Adresa> doInBackground(String... params) {
			List<Adresa> adrese = new ServiceProxy().pronadjiAdrese(params[0]);
			return adrese;
			
		}

		@Override
		protected void onPostExecute(List<Adresa> result) {
			PrikazAdresaActivity.this.adrese = result;
			if (result.size() == 0) {
				Toast.makeText(PrikazAdresaActivity.this, R.string.nema_rezultata, Toast.LENGTH_SHORT).show();
				progressBar.setVisibility(View.INVISIBLE);
				return;
			}
			PrikazAdresaActivity.this.setListAdapter(new AdreseAdapter(PrikazAdresaActivity.this, result));
			PrikazAdresaActivity.this.progressBar.setVisibility(View.INVISIBLE);
		}
	}
	
	class AdreseAdapter extends ArrayAdapter<Adresa> {

		public AdreseAdapter(Context context, List<Adresa> objects) {
			super(context, R.layout.row_address, R.id.adresa, objects);
		}

		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			View row = convertView;
			if (row == null) {
				row = getLayoutInflater().inflate(R.layout.row_address, parent, false);
			}
			Adresa adresa = PrikazAdresaActivity.this.adrese.get(position);
			TextView adresaTextView = (TextView)row.findViewById(id.adresa);
			adresaTextView.setText(adresa.getAdresa());
			//TextView mesto = (TextView)row.findViewById(id.mesto);
			//mesto.setText(adresa.getAddressLine(1));
			return row;
		}
		
		
		
	}

}
