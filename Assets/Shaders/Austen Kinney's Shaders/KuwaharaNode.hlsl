float3 mean[4] = {
	{0, 0, 0},
	{0, 0, 0},
	{0, 0, 0},
	{0, 0, 0}
};

float3 sigma[4] = {
	{0, 0, 0},
	{0, 0, 0},
	{0, 0, 0},
	{0, 0, 0}
};

float2 offsets[4] = {
	{-Radius, -Radius},
	{-Radius, 0},
	{0, -Radius},
	{0, 0}
};

float2 pos;
float3 color;


for (int i = 0; i < 4; i++)
{
	for (int x = 0; x <= Radius; x++)
	{
		for (int y = 0; y <= Radius; y++)
		{
			pos = float2(x, y) + offsets[i];
			float2 uvpos = UV + pos / ViewSize;

			color = SAMPLE_TEXTURE2D(SceneTexture, SS, uvpos);
			mean[i] += color;
			sigma[i] += color * color;
		}
	}
}

float n = pow(Radius + 1, 2);

float sigma_f;
float min = 1;

for (int i = 0; i < 4; i++)
{
	mean[i] /= n;
	sigma[i] = abs(sigma[i] / n - mean[i] * mean[i]);
	sigma_f = sigma[i].r + sigma[i].g + sigma[i].b;

	if (sigma_f < min)
	{
		min = sigma_f;
		color = mean[i];
	}
}
Output = color;