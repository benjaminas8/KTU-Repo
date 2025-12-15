%% (a) Astroidė
% x = 7cos^3(t), y = 7sin^3(t)

t = linspace(0, 2*pi, 2000);

x = 7*cos(t).^3;
y = 7*sin(t).^3;

figure;
plot(x, y, 'LineWidth', 1.5);
axis equal;
grid on;
xlabel('x');
ylabel('y');
title('(a) Astroidė: x=7cos^3t, y=7sin^3t');
%% (b) Epicikloidė (R=8, r=1) – 9 smailės
t = linspace(0,2*pi,6000);
x = 9*cos(t) - cos(9*t);
y = 9*sin(t) - sin(9*t);

figure; plot(x,y,'LineWidth',1.5); axis equal; grid on;
title('(b) x=9cost-cos9t, y=9sint-sin9t (Epicikloidė)');
xlabel('x'); ylabel('y');

%% (c) Racionalioji funkcija su trūkiais

x = linspace(-10, 10, 4000);

% Vengiam trūkio taškų x = -2 ir x = -4
y = (x.^3 + 2*x.^2 - x - 2) ./ (x.^2 + 6*x + 8);

figure;
plot(x, y, 'LineWidth', 1.2);
hold on;

% Vertikali asimptotė
xline(-4, '--r', 'x = -4');

% Pašalinamas trūkis (skylė)
plot(-2, 3/2, 'ro', 'MarkerSize', 8, 'LineWidth', 1.5);

ylim([-20 20]);
grid on;
xlabel('x');
ylabel('y');
title('(c) Racionalioji funkcija su trūkiais');
%% (d) Elipsė: x^2+4y^2-6x-8y-3=0
syms x y
F = x^2 + 4*y^2 - 6*x - 8*y - 3;

figure; fimplicit(F==0,[-10 10 -10 10],'LineWidth',1.5);
axis equal; grid on;
title('(d) x^2+4y^2-6x-8y-3=0 (Elipsė)');
xlabel('x'); ylabel('y');

%% (e) Parabolė: x^2-4x+4y-16=0
syms x y
G = x^2 - 4*x + 4*y - 16;

figure; fimplicit(G==0,[-10 10 -10 10],'LineWidth',1.5);
axis equal; grid on;
title('(e) x^2-4x+4y-16=0 (Parabolė)');
xlabel('x'); ylabel('y');

