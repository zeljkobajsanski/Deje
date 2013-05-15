package rs.zeks.deje;

import android.os.Bundle;
import android.app.Activity;
import android.content.ComponentName;
import android.content.Intent;
import android.view.DragEvent;
import android.view.Menu;
import android.view.View;
import android.view.View.OnDragListener;
import android.widget.EditText;
import android.widget.SeekBar;

public class MainActivity extends Activity {

	EditText nazivArtiklaEditText;
	EditText udaljenost;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		nazivArtiklaEditText = (EditText)findViewById(R.id.nazivArtikla);
		udaljenost = (EditText)findViewById(R.id.udaljenost);
		udaljenost.setText("2000");
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.activity_main, menu);
		return true;
	}
	
	public void pronadji(View view) {
		String nazivArtikla = nazivArtiklaEditText.getText().toString();
		Intent artikliIntent = new Intent(this, ArtikliActivity.class);
		artikliIntent.putExtra("NazivArtikla", nazivArtikla);
		artikliIntent.putExtra("Udaljenost", String.valueOf(getUdaljenost()));
		artikliIntent.putExtra("Latituda", "45.2485055");
		artikliIntent.putExtra("Longituda", "19.8762303");
		startActivity(artikliIntent);
	}
	
	private int getUdaljenost(){
		int udaljenost = Integer.parseInt(this.udaljenost.getText().toString());
		return udaljenost;
	}
	

}
